
using AuthenticationMechanism.tokenservice;
using EduNexBL.DTOs.AuthDtos;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

       
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly EduNexContext _context;

        private readonly TokenService _tokenService;

        
        public StudentController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, TokenService tokenService, EduNexContext context)

        {

            _userManager = userManager;

            _signInManager = signInManager;

        _tokenService = tokenService;
            _context = context;

    }

        [HttpPost("register/student")]

        public async Task<ActionResult> StudentRegister(StudentRegisterDto model)

        {
            // Validate the model

            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }
                   // Check if the email is already taken

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)

            {    ModelState.AddModelError("Email", "Email is already taken.");

                return BadRequest(ModelState);

            }


            // Create a new user with Identity Framework

            var newUser = new Student

            {

                FirstName = model.FirstName,

                LastName = model.LastName,

                PhoneNumber = model.PhoneNumber,

                gender = (Gender)Enum.Parse(typeof(Gender), model.gender),

                ParentPhoneNumber = model.ParentPhoneNumber,

                //CityId=model.CityId,

                Religion = model.Religion,

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
                await _userManager.AddToRoleAsync(newUser, "Student");

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



        [HttpPost("login/student")]

        public async Task<ActionResult<LoginUserDto>> StudentLogin(LoginUserDto model)

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


            // Check if the user is a student

            if (!await _userManager.IsInRoleAsync(user, "Student"))

            {

                ModelState.AddModelError("", "Invalid email or password.");

                return BadRequest(ModelState);

            }


            // Generate a token for the user

            var token = _tokenService.GenerateAccessToken(user.Id);


            // Return the token as a response

            return Ok(new

            {

                Token = token

            });

        }
    }
}