using AutoMapper;
using EduNexBL.Base;
using EduNexBL.DTOs;
using EduNexBL.DTOs.CourseDTOs;
using EduNexBL.ENums;
using EduNexBL.IRepository;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EduNexBL.Repository
{
    public class CourseRepo : Repository<Course>, ICourse
    {
        private readonly IMapper _mapper;
        private readonly EduNexContext _context;
        private readonly IConfiguration _configuration;

        public CourseRepo(EduNexContext dbContext, IMapper mapper, IConfiguration configuration) : base(dbContext)
        {
            _mapper = mapper;
            _context = dbContext;
            _configuration = configuration;
        }

        public async Task<ICollection<CourseMainData>> GetAllCoursesMainData()
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Subject)
                    .ThenInclude(s => s.Level)
                .ToListAsync();

            return _mapper.Map<List<CourseMainData>>(courses);
        }

        public async Task<ICollection<SubjectRDTO>> Getsubject()
        {
            var subjects = await _context.Subjects
               .Select(s => new SubjectRDTO { Id = s.Id, SubjectName = s.SubjectName })
               .ToListAsync();
            return (subjects);

        }
        public async Task<CourseDTO?> GetCourseById(int courseId)
        {
            var course = await _context.Courses
                          .Include(c => c.Teacher)
                          .Include(c => c.Subject)
                              .ThenInclude(s => s.Level)
                          .Include(c => c.Lectures)
                              .ThenInclude(l => l.Attachments)
                          .Include(c => c.Lectures)
                              .ThenInclude(l => l.Videos)
                            .Include(c => c.Lectures)
                              .ThenInclude(l => l.Exams)
                          .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return null; // Course with given ID not found

            return MapCourseToCourseDTO(course);
        }

        public CourseDTO MapCourseToCourseDTO(Course course)
        {
            return new CourseDTO
            {
                Id = course.Id,
                CourseName = course.CourseName,
                Thumbnail = course.Thumbnail,
                Price = course.Price,
                SubjectName = course.Subject?.SubjectName ?? "", // Assuming Subject has a Name property
                TeacherName = $"{course.Teacher?.FirstName} {course.Teacher?.LastName}", // Assuming Teacher has a Name property
                ProfilePhoto = course.Teacher?.ProfilePhoto ?? "", // Assuming Teacher has a ProfilePhoto property
                LevelName = course.Subject?.Level?.LevelName ?? "", // Assuming Subject has a Level property and Level has a Name property
                LectureList = course.Lectures.Select(MapLectureToLectureDTO).ToList(),
                teacherId = course.TeacherId,
                AboutTeacher = course.Teacher.AboutMe

            };
        }

        public LectureDto MapLectureToLectureDTO(Lecture lecture)
        {
            return new LectureDto
            {
                Id = lecture.Id,
                LectureTitle = lecture.LectureTitle,
                Price = lecture.Price,
                CourseId = lecture.CourseId,
                Videos = lecture.Videos?.Select(MapVideoToVideoDTO).ToList(),
                Attachments = lecture.Attachments?.Select(MapAttachmentToAttachmentDTO).ToList(),
                PreExam = lecture.Exams?.FirstOrDefault(e => e.Type == "PreExam")?.Id, // Get the first exam with type "PreExam"
                Assignment = lecture.Exams?.FirstOrDefault(a => a.Type == "Assignment")?.Id
            };
        }

        public VideoDTO MapVideoToVideoDTO(Video video)
        {
            return new VideoDTO
            {
                id = video.Id,
                VideoTitle = video.VideoTitle,
                VideoPath = video.VideoPath,
            };
        }

        public AttachmentDto MapAttachmentToAttachmentDTO(AttachmentFile attachment)
        {
            return new AttachmentDto
            {
                Id = attachment.Id,
                AttachmentTitle = attachment.AttachmentTitle,
                AttachmentPath = attachment.AttachmentPath,
            };
        }

        public async Task<decimal> GetCouponsValues(string[] couponCodes)
        {
            decimal discountValue = 0;
            foreach (var coupon in couponCodes)
            {
                var revievedCoupon = await _context.Coupon.FirstOrDefaultAsync(c => c.CouponCode == coupon && c.NumberOfUses > 0 
                                    && c.ExpirationDate > DateTime.Now && c.CouponType == CouponType.Discount_Coupon);
                if (revievedCoupon != null)
                {
                    discountValue += revievedCoupon.Value;
                }
            }
            return discountValue;
        }

        public async Task UpdateCouponUsageNumber(string[] couponCodes)
        {
            foreach (var coupon in couponCodes)
            {
                var revievedCoupon = await _context.Coupon.FirstOrDefaultAsync(c => c.CouponCode == coupon && c.NumberOfUses > 0 && c.ExpirationDate > DateTime.Now);
                if (revievedCoupon != null)
                {
                    revievedCoupon.NumberOfUses--;
                    _context.Coupon.Update(revievedCoupon);
                    _context.SaveChanges();
                }
            }
        }

        public async Task<EnrollmentResult> EnrollStudentInCourse(string studentId, int courseId, string[]? couponCodes)
        {
            try
            {
                var student = await _context.Students.SingleOrDefaultAsync(s => s.Id == studentId);
                var course = await _context.Courses.SingleOrDefaultAsync(c => c.Id == courseId);
                var studentWallet = await _context.Wallets.SingleOrDefaultAsync(w => w.OwnerId == studentId);

                if (student == null)
                {
                    return EnrollmentResult.StudentNotFound;
                }

                if (course == null)
                {
                    return EnrollmentResult.CourseNotFound;
                }

                //Check if the student is already enrolled in the course(if needed)
                if (await IsStudentEnrolledInCourse(studentId, courseId))
                {
                    return EnrollmentResult.AlreadyEnrolled;
                }

                if (couponCodes == null || couponCodes.Length == 0)
                {
                    //Checks student balance in the wallet
                    if (studentWallet.Balance < course.Price)
                    {
                        return EnrollmentResult.InsufficientBalance;
                    }
                    else
                    {
                        //log the purchased course
                        if (!await DistributePayment(studentId, courseId, ((decimal)course.Price))) return EnrollmentResult.Error;

                        // Create a new enrollment
                        var Enrollment = new StudentCourse
                        {
                            StudentId = studentId,
                            CourseId = courseId,
                            Enrolled = DateTime.UtcNow // or any appropriate timestamp
                        };

                        using var Process = await _context.Database.BeginTransactionAsync();
                        try
                        {
                            // Add the enrollment to the database
                            _context.StudentCourse.Add(Enrollment);

                            ////Update student balance in his wallet
                            studentWallet.Balance -= course.Price;
                            _context.Wallets.Update(studentWallet);
                            
                            _context.SaveChanges();

                            Process.Commit();
                            return EnrollmentResult.Success;
                        }
                        catch (Exception ex)
                        {
                            Process.RollbackAsync();
                            return EnrollmentResult.Error;
                        }                        
                    }
                }
                else
                {
                    var discountValue = await GetCouponsValues(couponCodes);
                    if (discountValue > 0)
                    {
                        if (studentWallet.Balance < (course.Price - discountValue))
                        {
                            return EnrollmentResult.InsufficientBalance;
                        }
                        else
                        {
                            //log the purchased course
                            if (!await DistributePayment(studentId, courseId, ((decimal)course.Price - discountValue))) return EnrollmentResult.Error;

                            // Create a new enrollment
                            var Enrollment = new StudentCourse
                            {
                                StudentId = studentId,
                                CourseId = courseId,
                                Enrolled = DateTime.Now 
                            };

                            using var Process = await _context.Database.BeginTransactionAsync();
                            try
                            {
                                // Add the enrollment to the database
                                _context.StudentCourse.Add(Enrollment);

                                //Update student balance in his wallet
                                studentWallet.Balance -= discountValue > course.Price ? 0 : (course.Price - discountValue);
                                _context.Wallets.Update(studentWallet);
                                _context.SaveChanges();

                                await UpdateCouponUsageNumber(couponCodes);

                                Process.Commit();
                                return EnrollmentResult.Success;
                            }
                            catch (Exception ex)
                            {
                                Process.Rollback();
                                return EnrollmentResult.Error;
                            }
                        }
                    }
                    else
                    {
                        return EnrollmentResult.InvalidCoupon;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return EnrollmentResult.Error;
            }
        }

        public async Task<bool> DistributePayment(string studentId, int courseId, decimal paidAmount)
        {
            decimal eduNexShare = decimal.Parse(_configuration["EduNexShare:SharePercentage"]);
            try
            {
                //Get the course being purchased
                var coursePurchased = await _context.Courses.FindAsync(courseId);
                if (coursePurchased == null) return false;

                //Get Teacher's Id
                var teacherId = await _context.Courses.Where(c => c.Id == courseId).Select(c => c.TeacherId).FirstOrDefaultAsync();
                if (teacherId == null || teacherId == String.Empty) return false;

                decimal eduNexAmount = paidAmount * eduNexShare;
                decimal teacherAmount = paidAmount * (1 - eduNexShare);

                //Get Teacher's Wallet
                var teacherWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.OwnerId == teacherId);
                if (teacherWallet == null)
                {
                    return false;
                }

                //Update Teacher's Wallet
                teacherWallet.Balance += teacherAmount;
                _context.Wallets.Update(teacherWallet);
                _context.SaveChanges();

                EduNexPurchaseLogs purchaseLog = new()
                {
                    CourseId = courseId,
                    SenderId = studentId,
                    ReceiverId = teacherId,
                    Amount = paidAmount,
                    AmountReceived = eduNexAmount,
                    IsCouponUsed = coursePurchased.Price > paidAmount,
                    CouponsValue = coursePurchased.Price > paidAmount ? coursePurchased.Price - paidAmount : null,
                    DateAdded = DateTime.UtcNow,
                };
                //Add a new log
                _context.EduNexPurchaseLogs.Add(purchaseLog);
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> IsStudentEnrolledInCourse(string studentId, int courseId)
        {
            // Check if there's any enrollment record matching the studentId and courseId
            var sc = await _context.StudentCourse
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            // If enrollment is not null, the student is enrolled in the course
            return sc != null;
        }
        public async Task<bool> IsTeacherRelatedToCourse(string teacherId, int courseId)
        {
            // Check if there's any course with the given courseId and associated with the given teacherId
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId && c.TeacherId == teacherId);

            // If course is not null, the teacher is related to the course
            return course != null;
        }

        public async Task<List<StudentCoursesDTO?>> CoursesEnrolledByStudent(string studentId)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return null;
            }

            var studentCoursesDTOs = student.StudentCourses
                .Select(sc => new StudentCoursesDTO
                {
                    CourseId = sc.CourseId,
                    CourseName = sc.Course.CourseName,
                    CourseThumbnail = sc.Course.Thumbnail
                })
                .ToList();

            return studentCoursesDTOs;
        }

        public async Task<int> CountEnrolledStudentsInCourse(int courseId)
        {
            int count = await _context.StudentCourse.CountAsync(sc => sc.CourseId == courseId);
            return count;
        }
        public async Task<int> CountCourseLectures(int courseId)
        {
            int count = await _context.Lectures.CountAsync(l => l.CourseId == courseId);
            return count;
        }

        public async Task<List<TeacherCoursesViewDTO>> GetTeacherCourses(string teacherId)
        {
            var courses = await _context.Courses
                .Where(c => c.TeacherId == teacherId)
                .Include(c => c.Subject)
                .Select(course => new TeacherCoursesViewDTO
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    Thumbnail = course.Thumbnail,
                    Price = course.Price,
                    SubjectId = course.SubjectId,
                    SubjectName = course.Subject.SubjectName,
                    CreatedAt = course.CreatedAt.ToString(),
                    UpdatedAt = course.UpdatedAt.ToString()
                })
                .ToListAsync();

            return courses;
        }

        public async Task<List<MostBuyedCoursesDTO>> GetCoursesOrderedByEnrollment()
        {
            var CoursesEnrolledIdList = await _context.StudentCourse.Select(sc => sc.CourseId).ToListAsync();

            var CoursesList = new List<CourseDTO>();
            
            var result = new List<MostBuyedCoursesDTO>();

            foreach (var CourseId in CoursesEnrolledIdList)
            {
                var course = await GetCourseById(CourseId);
                CoursesList.Add(course);
            }

            foreach (var course in CoursesList)
            {
                MostBuyedCoursesDTO EnrolledCourse = new MostBuyedCoursesDTO();

                EnrolledCourse = new MostBuyedCoursesDTO
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    Thumbnail = course.Thumbnail,
                    Price = course.Price,
                    SubjectName = course.SubjectName,
                    TeacherId = course.teacherId,
                    TeacherName = course.TeacherName, // Assuming you have a method to get the teacher's name
                    ProfilePhoto = course.ProfilePhoto, // Assuming you have a method to get the teacher's profile photo
                    LevelName = course.LevelName,
                    EnrollmentCount = await CountEnrolledStudentsInCourse(course.Id) // Assuming you have a method to get the enrollment count
                };
                result.Add(EnrolledCourse);                
            }
            var OrderedResult = result.OrderByDescending(ord => ord.EnrollmentCount).ToList();
            return OrderedResult;

            //var coursesWithEnrollments = await _context.StudentCourse
            //    .GroupBy(sc => sc.CourseId)
            //    .Select(x => new
            //    {
            //        CourseId = x.Key,
            //        EnrollmentCount = x.Count()
            //    })
            //    .ToListAsync();

            //var result = await _context.Courses
            //    .Join(
            //        coursesWithEnrollments,
            //        course => course.Id,
            //        enrollment => enrollment.CourseId,
            //        (course, enrollment) => new MostBuyedCoursesDTO
            //        {
            //            Id = course.Id,
            //            Name = course.CourseName,
            //            Thumbnail = course.Thumbnail, 
            //            EnrollmentCount = enrollment.EnrollmentCount
            //        }
            //    )
            //    .OrderByDescending(ord => ord.EnrollmentCount)
            //    .ToListAsync();

            //return result;
        }

        //public async Task<List<MostBuyedCoursesDTO>> GetCoursesOrderedByCreateionDateDescending()
        //{
        //    var courses = await _context.Courses
        //    .Select(course => new MostBuyedCoursesDTO
        //    {
        //        Id = course.Id,
        //        Name = course.CourseName,
        //        CreationDate = course.CreatedAt
        //    })
        //    .OrderByDescending(c => c.CreationDate)
        //    .ToListAsync();

        //    return courses;
        //}

        //public async Task<List<MostBuyedCoursesDTO>> GetCoursesOrderedByCreateionDateAscending()
        //{
        //    var courses = await _context.Courses
        //    .Select(course => new MostBuyedCoursesDTO
        //    {
        //        Id = course.Id,
        //        Name = course.CourseName,
        //        CreationDate = course.CreatedAt
        //    })
        //    .OrderBy(c => c.CreationDate)
        //    .ToListAsync();

        //    return courses;
        //}
    }
}

