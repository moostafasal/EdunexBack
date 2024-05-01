using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakeDataController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EduNexContext _dbContext;
        private readonly ILogger<FakeDataController> _logger; // Add logger

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
                for (int i = 0; i < 10; i++)
                {
                    var fakeTeacher = new Teacher
                    {
                        FirstName = "John" + i.ToString(),
                        LastName = "Doe" + i.ToString(),
                        PhoneNumber = "123456789" + i.ToString(), // Set a non-null value for PhoneNumber
                        DateOfBirth = new DateTime(1985, 5, 10),
                        gender = Gender.Male,
                        // Add other properties
                        ProfilePhoto = "john.jpg",
                        Description = "Math teacher with 10 years of experience",
                        FacebookAccount = "john.doe",
                        AboutMe = "I love teaching and helping students understand complex concepts.",
                        AccountNote = "Account created on April 30, 2024",
                        Address = "123 Main Street, City, Country",
                        NationalId = "123456789",
                        Status = TeacherStatus.Approved
                        // Add other properties as needed
                    };

                    _dbContext.Teachers.Add(fakeTeacher);

                    // Create a fake user account with default password
                    var user = new ApplicationUser
                    {
                        FirstName = fakeTeacher.FirstName,
                        LastName = fakeTeacher.LastName,
                        UserName = $"john.doe{i + 1}@example.com",
                        Email = $"john.doe{i + 1}@example.com",
                        PhoneNumber = fakeTeacher.PhoneNumber // Set PhoneNumber for ApplicationUser
                    };

                    // Check if PhoneNumber is null
                    if (string.IsNullOrEmpty(user.PhoneNumber))
                    {
                        // Log error and continue to the next iteration
                        _logger.LogError($"PhoneNumber is null for user {user.UserName}");
                        continue;
                    }

                    var result = await _userManager.CreateAsync(user, "DefaultPassword123!"); // Change to your default password

                    if (result.Succeeded)
                    {
                        // Assign the teacher role
                        await _userManager.AddToRoleAsync(user, "Teacher");
                    }
                    else
                    {
                        // Log errors
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



        [HttpGet("teachers")] // Define the HTTP route for retrieving teachers
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


        [HttpPost("seed-subjects")]
        public IActionResult SeedSubjects()
        {
            try
            {
                var subjIds = _dbContext.Levels.Select(C => C.Id).ToList();

                var subjects = new List<Subject>();

                for (int i = 1; i <= 10; i++)
                {
                    var level = subjIds[new Random().Next(0, subjIds.Count)];

                    subjects.Add(new Subject { SubjectName = $"Subject {i}", LevelId = level, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false });
                }

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
                var coursesIds = _dbContext.Courses.Select(C => C.Id).ToList();


                var lectures = new List<Lecture>();

                for (int i = 1; i <= 10; i++)
                {
                    var randomCourse = coursesIds[new Random().Next(0, coursesIds.Count)];

                    lectures.Add(new Lecture { LectureTitle = $"Lecture {i}", Price = 9.99m, CourseId = randomCourse, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false });
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
                // Fetch teacher IDs from the database
                var teacherIds = _dbContext.Teachers.Select(t => t.Id).ToList();
                var subIds = _dbContext.Subjects.Select(S => S.Id).ToList();



                if (teacherIds.Count == 0)
                {
                    return BadRequest("No teachers found in the database. Seed teachers before seeding courses.");
                }

                var courses = new List<Course>();

                // Seed 10 courses
                for (int i = 1; i <= 10; i++)
                {
                    // Get a random teacher ID
                    var randomTeacherId = teacherIds[new Random().Next(0, teacherIds.Count)];
                    var randomsubject = subIds[new Random().Next(0, subIds.Count)];

                    courses.Add(new Course
                    {
                        CourseName = $"Course {i}",
                        Thumbnail = "course_thumbnail.jpg",
                        CourseType = CourseType.Scientific,
                        Price = 29.99m,
                        SubjectId = randomsubject,
                        TeacherId = randomTeacherId, // Assign a random teacher ID
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false
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




    }
}
