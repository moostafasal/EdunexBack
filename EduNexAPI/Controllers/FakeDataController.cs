using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakeDataController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EduNexContext _dbContext;
        private readonly ILogger<FakeDataController> _logger;

        public FakeDataController(UserManager<ApplicationUser> userManager, EduNexContext dbContext, ILogger<FakeDataController> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost("seed-teachers")]
        public async Task<IActionResult> SeedTeachersManually()
        {
            try
            {
                var maleFirstNames = new List<string> { "محمد", "أحمد", "علي", "يوسف", "عمر", "أيمن", "مصطفى", "خالد", "طارق", "حسين" };
                var femaleFirstNames = new List<string> { "فاطمة", "زينب", "مريم", "آمنة", "سارة", "نور", "آية", "ليلى", "داليا", "رنا" };
                var lastNames = new List<string> { "السعيد", "المصري", "العراقي", "الجزائري", "التونسي", "السعودي", "اللبناني", "الإماراتي", "البحريني", "الكويتي" };

                for (int i = 0; i < 10; i++)
                {
                    var gender = (i % 2 == 0) ? Gender.Male : Gender.Female;
                    var firstName = (gender == Gender.Male) ? maleFirstNames[i] : femaleFirstNames[i];
                    var lastName = lastNames[i];
                    var phoneNumber = $"0101234567{i}";

                    var fakeTeacher = new Teacher
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNumber = phoneNumber,
                        DateOfBirth = new DateTime(1980, 1, 1),
                        gender = gender,
                        ProfilePhoto = "https://via.placeholder.com/225X325",
                        Description = $"مدرس في مادة الرياضيات بخبرة أكثر من {10 + i} سنة.",
                        FacebookAccount = $"{firstName}.{lastName}{i}",
                        AboutMe = $"أسعى لتقديم تعليم ممتاز وتحفيز الطلاب على تحقيق أهدافهم في مادة الرياضيات.",
                        AccountNote = $"تم إنشاء الحساب في {DateTime.Now:yyyy-MM-dd}",
                        Address = "شارع النيل، القاهرة، مصر",
                        NationalId = $"123456789{i}",
                        Status = TeacherStatus.Approved
                    };

                    _dbContext.Teachers.Add(fakeTeacher);

                    var user = new ApplicationUser
                    {
                        FirstName = fakeTeacher.FirstName,
                        LastName = fakeTeacher.LastName,
                        UserName = $"{firstName.ToLower()}.{lastName.ToLower()}{i + 1}@example.com",
                        Email = $"{firstName.ToLower()}.{lastName.ToLower()}{i + 1}@example.com",
                        PhoneNumber = fakeTeacher.PhoneNumber
                    };

                    if (string.IsNullOrEmpty(user.PhoneNumber))
                    {
                        _logger.LogError($"PhoneNumber is null for user {user.UserName}");
                        continue;
                    }

                    var result = await _userManager.CreateAsync(user, "DefaultPassword123!");

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Teacher");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();

                return Ok("Fake data seeded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding fake teachers.");
                return StatusCode(500, "An error occurred while seeding fake teachers.");
            }
        }

        [HttpPost("seed-students")]
        public async Task<IActionResult> SeedStudentsManually()
        {
            try
            {
                var maleFirstNames = new List<string> { "علي", "محمد", "أحمد", "يوسف", "خالد", "عمر", "مصطفى", "طارق", "عماد", "حسين" };
                var femaleFirstNames = new List<string> { "مريم", "فاطمة", "زينب", "آية", "سارة", "نور", "ليلى", "آمنة", "ملك", "أمل" };
                var lastNames = new List<string> { "السعيد", "المصري", "العراقي", "الجزائري", "التونسي", "السعودي", "اللبناني", "الإماراتي", "البحريني", "الكويتي" };

                for (int i = 0; i < 10; i++)
                {
                    var gender = (i % 2 == 0) ? Gender.Male : Gender.Female;
                    var firstName = (gender == Gender.Male) ? maleFirstNames[i] : femaleFirstNames[i];
                    var lastName = lastNames[i];
                    var phoneNumber = $"0101234567{i}";

                    var fakeStudent = new Student
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNumber = phoneNumber,
                        DateOfBirth = new DateTime(2005, 1, 1),
                        gender = gender,
                        ParentPhoneNumber = $"0109876543{i}",
                        Religion = "مسلم",
                        LevelId = 1,
                        City = "القاهرة",
                        Address = "شارع المريوطية، الجيزة، مصر",
                        NationalId = $"123456789{i}"
                    };

                    _dbContext.Students.Add(fakeStudent);

                    var user = new ApplicationUser
                    {
                        FirstName = fakeStudent.FirstName,
                        LastName = fakeStudent.LastName,
                        UserName = $"{firstName.ToLower()}.{lastName.ToLower()}{i + 1}@example.com",
                        Email = $"{firstName.ToLower()}.{lastName.ToLower()}{i + 1}@example.com",
                        PhoneNumber = fakeStudent.PhoneNumber
                    };

                    if (string.IsNullOrEmpty(user.PhoneNumber))
                    {
                        _logger.LogError($"PhoneNumber is null for user {user.UserName}");
                        continue;
                    }

                    var result = await _userManager.CreateAsync(user, "DefaultPassword123!");

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Student");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(error.Description);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();

                return Ok("Fake data seeded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding fake students.");
                return StatusCode(500, "An error occurred while seeding fake students.");
            }
        }

        [HttpPost("seed-subjects")]
        public IActionResult SeedSubjects()
        {
            try
            {
                var subjects = new List<Subject>
                {
                    new Subject { SubjectName = "اللغة العربية" , SubjectType = "General" , LevelId = 3},
                    new Subject { SubjectName = "التاريخ" , SubjectType = "Scientific" , LevelId = 3},
                    new Subject { SubjectName = "الجغرافيا" , SubjectType = "Literature" , LevelId = 3},
                    new Subject { SubjectName = "الفيزياء" , SubjectType = "Scientific", LevelId = 3},
                    new Subject { SubjectName = "الكيمياء" , SubjectType = "Scientific" , LevelId = 3},
                    new Subject { SubjectName = "الفلسفة" , SubjectType = "Literature" , LevelId = 3},
                    new Subject { SubjectName = "اللغة الإنجليزية",  SubjectType = "General" , LevelId = 3},
                    new Subject { SubjectName = "الاقتصاد" , SubjectType = "Literature", LevelId = 3}
                };

                _dbContext.Subjects.AddRange(subjects);
                _dbContext.SaveChanges();

                return Ok("Subjects seeded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding subjects.");
                return StatusCode(500, "An error occurred while seeding subjects.");
            }
        }

        [HttpPost("seed-lectures")]
        public IActionResult SeedLectures()
        {
            try
            {
                var lectures = new List<Lecture>();

                // Get the list of all existing course IDs
                var courseIds = _dbContext.Courses.Select(c => c.Id).ToList();

                if (courseIds.Count == 0)
                {
                    return BadRequest("No courses found in the database. Seed courses before seeding lectures.");
                }

                for (int i = 1; i <= 10; i++)
                {
                    var randomSubject = _dbContext.Subjects.OrderBy(s => Guid.NewGuid()).FirstOrDefault();
                    var randomTeacher = _dbContext.Teachers.OrderBy(t => Guid.NewGuid()).FirstOrDefault();
                    var rand = new Random();

                    var randomPrice = Math.Round((decimal)(rand.NextDouble() * (50.00 - 5.00) + 5.00), 2);

                    // Select a random course ID
                    var randomCourseId = courseIds[rand.Next(0, courseIds.Count)];

                    lectures.Add(new Lecture
                    {
                        LectureTitle = $"محاضرة {i}",
                        Price = randomPrice,
                        CourseId = randomCourseId // Assign a random course ID to the lecture
                    });
                }

                _dbContext.Lectures.AddRange(lectures);
                _dbContext.SaveChanges();

                return Ok("Lectures seeded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding lectures.");
                return StatusCode(500, "An error occurred while seeding lectures.");
            }
        }


        [HttpPost("seed-courses")]
        public IActionResult SeedCourses()
        {
            try
            {
                var rand = new Random();

                // Get the list of all subject IDs
                var subjectIds = _dbContext.Subjects.Select(s => s.Id).ToList();

                // Get the list of all teacher IDs
                var teacherIds = _dbContext.Teachers.Select(t => t.Id).ToList();

                var courses = new List<Course>();

                for (int i = 1; i <= 10; i++)
                {
                    // Generate a random index to select a random subject ID
                    var randomSubjectIndex = rand.Next(0, subjectIds.Count);

                    // Generate a random index to select a random teacher ID
                    var randomTeacherIndex = rand.Next(0, teacherIds.Count);

                    courses.Add(new Course
                    {
                        CourseName = $"دورة {i}",
                        Thumbnail = "https://via.placeholder.com/325X225",
                        Price = Math.Round((decimal)(rand.NextDouble() * (50.00 - 5.00) + 5.00), 2), // Random price between 5.00 and 50.00
                        SubjectId = subjectIds[randomSubjectIndex], // Select a random subject ID
                        TeacherId = teacherIds[randomTeacherIndex], // Select a random teacher ID
                    });
                }

                _dbContext.Courses.AddRange(courses);
                _dbContext.SaveChanges();

                return Ok("Courses seeded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding courses.");
                return StatusCode(500, "An error occurred while seeding courses.");
            }
        
        }

        [HttpGet("teachers")]
        public IActionResult GetTeachers()
        {
            try
            {
                var teachers = _dbContext.Teachers.ToList();
                return Ok(teachers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving teachers.");
                return StatusCode(500, "An error occurred while retrieving teachers.");
            }
        }

        // Add other methods as needed...

    }
}
