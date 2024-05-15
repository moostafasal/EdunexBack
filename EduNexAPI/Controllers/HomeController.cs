using EduNexBL.IRepository;
using EduNexBL.UnitOfWork;
using EduNexDB.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EduNexContext _Context;
        private readonly IAdminRepository _adminRepository;

        public HomeController(IUnitOfWork unitOfWork, EduNexContext context, IAdminRepository adminRepository)
        {
            _unitOfWork = unitOfWork;
            _Context = context;
            _adminRepository = adminRepository;
        }

        [HttpGet("GetStudentsOrderedByScore")]
        public async Task<IActionResult> GetStudentTotalScores()
        {
            try
            {
                var studentScoreOredered = await _unitOfWork.StudentExamRepo.GetStudentTotalScores();
                return Ok(studentScoreOredered);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCoursesOrderedByEnrollment")]
        public async Task<IActionResult> GetCoursesOrderedByEnrollment()
        {
            try
            {
                var orderedCoursesList = await _unitOfWork.CourseRepo.GetCoursesOrderedByEnrollment();
                if (orderedCoursesList.IsNullOrEmpty())
                {
                    return NotFound();
                }
                else
                {
                    return Ok(orderedCoursesList);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

<<<<<<< HEAD
        [HttpGet("GetCoursesOrderedByCreateionDateDescending")]
        public async Task<IActionResult> GetCoursesOrderedByCreateionDateDescending()
        {
            try
            {
                var orderedCoursesList = await _unitOfWork.CourseRepo.GetCoursesOrderedByCreationDateDescending();
                if (orderedCoursesList.IsNullOrEmpty())
                {
                    return NotFound();
                }
                else
                {
                    return Ok(orderedCoursesList);
=======
        [HttpGet("GetCoursesCount")]
        public async Task<IActionResult> GetCoursesCount()
        {
            try
            {
                int CoursesCount = await _Context.Courses.CountAsync();
                if (CoursesCount > 0)
                {
                    return Ok(CoursesCount);
                }
                else
                {
                    return NotFound();
>>>>>>> 4e5b094562287e295359d25445c96289cb619958
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

<<<<<<< HEAD
=======
        [HttpGet("GetStudentCount")]
        public async Task<IActionResult> GetStudentCount()
        {
            try
            {
                int StudentsCount = await _Context.Students.CountAsync();
                if (StudentsCount > 0)
                {
                    return Ok(StudentsCount);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetTeachersCount")]
        public async Task<IActionResult> GetTeachersCount()
        {
            try
            {
                int TeachersCount = await _Context.Teachers.CountAsync();
                if (TeachersCount > 0)
                {
                    return Ok(TeachersCount);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("GetLecturesCount")]
        public async Task<IActionResult> GetLecturesCount()
        {
            try
            {
                int LecturesCount = await _Context.Lectures.CountAsync();
                if (LecturesCount > 0)
                {
                    return Ok(LecturesCount);
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
>>>>>>> 4e5b094562287e295359d25445c96289cb619958
    }
}
