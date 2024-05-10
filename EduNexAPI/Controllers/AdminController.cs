using EduNexBL;
using EduNexBL.DTOs;
using EduNexBL.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class AdminController : ControllerBase
{
    private readonly IAdminRepository _adminRepository;

    public AdminController(IAdminRepository adminRepository )
    {
        _adminRepository = adminRepository;
    }


    [HttpGet("teachers")]
    public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachers()
    {
        var teachers = await _adminRepository.GetTeachersAsync();
        return Ok(teachers);
    }
    [HttpGet("teachers/Pending")]
    public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachersPending()
    {
        var teachers = await _adminRepository.GetTeachersPendingAsync();
        return Ok(teachers);
    }

    [HttpPut("teachers/approve/{id}")]
    public async Task<IActionResult> ApproveTeacher(string id)
    {
        var result = await _adminRepository.ApproveTeacherAsync(id);
        if (result)
            return Ok("Teacher approved successfully.");
        return NotFound();
    }


    [HttpPut("teachers/reject/{id}")]
    public async Task<IActionResult> RejectTeacher(string id)
    {
        var result = await _adminRepository.RejectTeacherAsync(id);
        if (result)
            return Ok("Teacher rejected successfully.");
        return NotFound();  
    }

    [HttpGet("teacher/{id}")]
    public async Task<ActionResult<TeacherDto>> GetTeacherById(string id)
    {
        var teacher = await _adminRepository.GetTeacherByIdAsync(id);

        if (teacher == null)
            return NotFound();

        //var newTeacher = new TeacherDto
        //{
        //    AboutMe = teacher.AboutMe,
        //    City = teacher.City,
        //    FacebookAccount = teacher.FacebookAccount,
        //    PhoneNumber = teacher.PhoneNumber,
        //    DateOfBirth = teacher.DateOfBirth,
        //    AccountNote = teacher.AccountNote,
        //    Address = teacher.Address,
        //    Age = teacher.Age,
        //    Email = teacher.Email,
        //    FirstName = teacher.FirstName,
        //    LastName = teacher.LastName,
        //    gender = teacher.gender,
        //    Id = teacher.Id,
        //    NationalId = teacher.NationalId,
        //    ProfilePhoto = teacher.ProfilePhoto,
        //    subject = teacher.subject
        //};
        return Ok(teacher);
    }

    [HttpGet("students/{id}")]
    public async Task<ActionResult<StudentDto>> GetStudentById(string id)
    {
        var student = await _adminRepository.GetStudentByIdAsync(id);
        if (student == null)
            return NotFound();
        return Ok(student);
    }

    [HttpPost("search")]
    public async Task<ActionResult<IEnumerable<UserDto>>> SearchUsers(SearchQuery query)
    {
        var users = await _adminRepository.SearchUsersAsync(query);
        return Ok(users);
    }

    [HttpGet("students")]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
    {
        var students = await _adminRepository.GetStudentsAsync();
        return Ok(students);
    }

    [HttpGet("teachers/count")]
    public async Task<ActionResult> GetTeachersCount()
    {
        try
        {
            var teachers = await _adminRepository.GetTeachersAsync();
            var count = teachers.Count();
            return Ok(new { count });

        }
        catch(Exception ex) 
        {

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An unexpected error occurred while processing your request." });
        
        }
 
    }

    [HttpGet("students/count")]
    public async Task<ActionResult> GetStudentsCount()
    {
        try
        {
            var std = await _adminRepository.GetStudentsAsync();
            var count = std.Count();
            return Ok(new { count });

        }
        catch (Exception ex)
        {

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { error = "An unexpected error occurred while processing your request." });

        }

    }

    [HttpDelete("Student/{id}")]
    public async Task<IActionResult> DeleteStudent (string id)
    {
        try
        {
            //if (id <= 0)
            //{
            //    return BadRequest("Invalid student ID. ID must be greater than zero.");
            //}

            await _adminRepository.DeleteStudentAsync(id);
            return Ok("Student Deleted Sucessfully");

        }
        catch (Exception e)
        {
            return StatusCode(500, "An unexpected error occurred. Please try again later.");

        }
    }

    [HttpDelete("Teacher/{id}")]
    public async Task<IActionResult> DeleteTeacher(string id)
    {
        try
        {
            //if (id <= 0)
            //{
            //    return BadRequest("Invalid Teacher ID. ID must be greater than zero.");
            //}

            await _adminRepository.DeleteTeacherAsync(id);
            return Ok("Teacher Deleted Sucessfully");

        }
        catch (Exception e)
        {
            return StatusCode(500, "An unexpected error occurred. Please try again later.");

        }
    }
}