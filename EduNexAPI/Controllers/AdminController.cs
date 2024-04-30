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
}