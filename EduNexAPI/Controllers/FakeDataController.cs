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
    }
}
