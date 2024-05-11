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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduNexBL.Repository
{
    public class CourseRepo : Repository<Course>, ICourse
    {
        private readonly IMapper _mapper;
        private readonly EduNexContext _context;

        public CourseRepo(EduNexContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
            _context = dbContext;
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
                var revievedCoupon = await _context.Coupon.FirstOrDefaultAsync(c => c.CouponCode == coupon && c.NumberOfUses > 0 && c.ExpirationDate > DateTime.Now);
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
                        ////Update student balance in his wallet
                        studentWallet.Balance -= course.Price;
                        _context.Wallets.Update(studentWallet);
                        _context.SaveChanges();

                        // Create a new enrollment
                        var Enrollment = new StudentCourse
                        {
                            StudentId = studentId,
                            CourseId = courseId,
                            Enrolled = DateTime.Now // or any appropriate timestamp
                        };

                        // Add the enrollment to the database
                        _context.StudentCourse.Add(Enrollment);
                        _context.SaveChanges();

                        return EnrollmentResult.Success;
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
                            //Update student balance in his wallet
                            studentWallet.Balance -= (course.Price - discountValue);
                            _context.Wallets.Update(studentWallet);
                            await _context.SaveChangesAsync();
                            await UpdateCouponUsageNumber(couponCodes);

                            // Create a new enrollment
                            var Enrollment = new StudentCourse
                            {
                                StudentId = studentId,
                                CourseId = courseId,
                                Enrolled = DateTime.Now // or any appropriate timestamp
                            };

                            // Add the enrollment to the database
                            _context.StudentCourse.Add(Enrollment);
                            _context.SaveChanges();

                            return EnrollmentResult.Success;
                        }
                    }
                    else
                    {
                        return EnrollmentResult.InvalidCoupon;
                    }
                }

                // Create a new enrollment
                //var enrollment = new StudentCourse
                //{
                //    StudentId = studentId,
                //    CourseId = courseId,
                //    Enrolled = DateTime.Now // or any appropriate timestamp
                //};

                //return EnrollmentResult.Success;
            }
            catch (Exception ex)
            {
                // Log the exception
                return EnrollmentResult.Error;
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
                    SubjectName = course.Subject.SubjectName,
                    CreatedAt = course.CreatedAt.ToString(),
                    UpdatedAt = course.UpdatedAt.ToString()
                })
                .ToListAsync();

            return courses;
        }

    }
}

