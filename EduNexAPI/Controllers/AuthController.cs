using AuthenticationMechanism.tokenservice;
using EduNexBL.DTOs.AuthDtos;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "User logged out successfully." });
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDto model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the email and password are valid
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return BadRequest(ModelState);
            }

            // Get the user
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return BadRequest(ModelState);
            }

            var token = await _tokenService.GenerateAccessToken(user.Id);
            // Check if the user is a teacher
            if (await _userManager.IsInRoleAsync(user, "Teacher"))
            {
                // Check the status of the teacher
                var teacher = (Teacher)user;
                if (teacher.Status == TeacherStatus.Pending)
                {

                    var response = new
                    {
                        Teacher = teacher.Id,
                        Token = token,


                        Message = "pending"
                        
                    };

                return Ok(response);
                }
                else if (teacher.Status == TeacherStatus.Rejected)
                {
                    var response = new
                    {
                        Teacher = teacher.Id,
                        Token = token,


                        Message = "Rejected"

                    };
                }
            }

            // Generate a token for the user

            // Return the token as a response
            return Ok(new
            {
                Token = token
            });
        }



        [HttpPost("Admin")]
        public async Task<ActionResult> AdminRegister(RegisterTeacherDto model)

        {
            // Validate the model

            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }
            // Check if the email is already taken

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)

            {
                ModelState.AddModelError("Email", "Email is already taken.");

                return BadRequest(ModelState);

            }


            // Create a new user with Identity Framework

            var newUser = new ApplicationUser

            {

                FirstName = model.FirstName,

                LastName = model.LastName,

                PhoneNumber = model.PhoneNumber,

                gender = (Gender)Enum.Parse(typeof(Gender), model.Gender),


                DateOfBirth = model.DateOfBirth,

                Address = model.Address,

                NationalId = model.NationalId,

                Email = model.Email,

                UserName = model.Email,


                //LevelId = model.LevelId

            };


            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {
                // Add the user to the "Student" role
                await _userManager.AddToRoleAsync(newUser, "Admin");

                // Generate a token for the new user
                var token = await _tokenService.GenerateAccessToken(newUser.Id); // Ensure to await token generation

                // Return the created user and the token as a response
                return Ok(new
                {
                    User = newUser, // Optionally return user information
                    Token = token
                });
            }
            else
            {
                // If the user creation failed, return a bad request response with the errors
                return BadRequest(result.Errors);
            }

        }



    }
}
