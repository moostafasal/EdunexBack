
using AuthenticationMechanism.tokenservice;
using EduNexBL.DTOs;
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
            {
                ModelState.AddModelError("Email", "Email is already taken.");
                return BadRequest(ModelState);
            }

            var newUser = new Student
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                gender = (Gender)Enum.Parse(typeof(Gender), model.gender),
                ParentPhoneNumber = model.ParentPhoneNumber,
                Religion = model.Religion,
                DateOfBirth = model.DateOfBirth,
                City = model.City,
<<<<<<< HEAD
=======

                NationalId = model.NationalId,


>>>>>>> 2c3693e679df8097a0aecc5a21fbc8efb4ce7ade
                Email = model.Email,
                UserName = model.Email,
                LevelId = model.LevelId,
                Address = model.Address
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (result.Succeeded)
            {
                var wallet = new Wallet
                {
                    OwnerId = newUser.Id,
                    Balance = 0,
                    OwnerType = "Student"
                };

                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();

                newUser.walletId = wallet.WalletId;

                await _userManager.AddToRoleAsync(newUser, "Student");

                var token = await _tokenService.GenerateAccessToken(newUser.Id);

                return Ok(new
                {
                    User = newUser,
                    Token = token
                });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }


        [HttpGet("GetStudentById/{id}")]
        public async Task<ActionResult<StudentDto1>> GetStudentById(string id)
        {
            var student = await _context.Students
                .Include(s => s.Level)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var studentDto = new StudentDto1
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                ParentPhoneNumber = student.ParentPhoneNumber,
                Gender = student.gender.ToString(),
                Religion = student.Religion,
                LevelId = student.LevelId,
                LevelName = student.Level != null ? student.Level.LevelName : null,
                Address = student.Address,
                City = student.City,
                DateOfBirth = student.DateOfBirth,
                PhoneNumber = student.PhoneNumber

            };

            return Ok(studentDto);
        }



        //get studnt by eamil :
        [HttpGet("Get-Student/{mail}")]
        public async Task<ActionResult<StudentDto1>> GetStudentByemail(string Email)
        {
            var student = await _context.Students
                .Include(s => s.Level)
                .FirstOrDefaultAsync(s => s.Email == Email);

            if (student == null)
            {
                return NotFound();
            }

            var studentDto = new StudentDto1
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                ParentPhoneNumber = student.ParentPhoneNumber,
                Gender = student.gender.ToString(),
                Religion = student.Religion,
                LevelId = student.LevelId,
                LevelName = student.Level != null ? student.Level.LevelName : null,

            };

            return Ok(studentDto);
        }


        [HttpPut("Student/{id}")]
        public async Task<IActionResult> UpdateStudent(string id, CustomStudentDto customStudentDto)
        {
            if (string.IsNullOrEmpty(id) || customStudentDto == null)
            {
                return BadRequest("Invalid data provided.");
            }

            var existingStudent = await _context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            if (ModelState.IsValid)
            {
                existingStudent.FirstName = customStudentDto.FirstName;
                existingStudent.LastName = customStudentDto.LastName;
                existingStudent.ParentPhoneNumber = customStudentDto.ParentPhoneNumber;
                existingStudent.Religion = customStudentDto.Religion;
                existingStudent.LevelId = customStudentDto.LevelId;
                existingStudent.gender = (Gender)Enum.Parse(typeof(Gender), customStudentDto.Gender);
                existingStudent.Address = customStudentDto.address;
                existingStudent.DateOfBirth = customStudentDto.birthDate;
                existingStudent.City= customStudentDto.city;
                existingStudent.PhoneNumber = customStudentDto.PhoneNumber;
                try
                {
                    _context.Students.Update(existingStudent); // Mark the entity as modified
                    await _context.SaveChangesAsync();
                    return Ok(existingStudent); // Return the updated student
                }
                catch (DbUpdateException)
                {
                    // Log the exception or handle it accordingly
                    return StatusCode(500, "Failed to update student.");
                }
            }
            else
            {
                return BadRequest(ModelState.Values.SelectMany(i=>i.Errors).Select(s=>s.ErrorMessage).ToList());
            }
        }



        [HttpGet("GetStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _context.Students.ToListAsync();

            if (students == null)
            {
                return BadRequest("No Found Data");
            }
            else
            {
                var studentsData = students.Select(i => new StudentDto
                {
                    Email = i.Email,
                    ParentPhoneNumber = i.ParentPhoneNumber,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Gender = i.gender,
                    LevelId = i.LevelId,
                    Id = i.Id,
                    LevelName = i.Level != null ? i.Level.LevelName : null,
                    Religion = i.Religion
                });
                return Ok(studentsData);
            }
        }


    }
}