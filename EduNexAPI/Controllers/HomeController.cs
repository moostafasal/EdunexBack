using EduNexBL.IRepository;
using EduNexBL.UnitOfWork;
using EduNexDB.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("GetCoursesCount")]
        public async Task<IActionResult> GetCoursesCount()
        {
            try
            {
                var Courses = await _unitOfWork.CourseRepo.GetAll();
                int CoursesCount = Courses.Count();
                if (CoursesCount > 0)
                {
                    return Ok(CoursesCount);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetStudentCount")]
        public async Task<IActionResult> GetStudentCount()
        {
            try
            {
                var Students = await _unitOfWork.StudentRepo.GetAll();
                int StudentsCount = Students.Count();
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
        
        [HttpPost("GetTeachersCount")]
        public async Task<IActionResult> GetTeachersCount()
        {
            try
            {
                var Teachers = await _adminRepository.GetTeachersAsync();
                int TeachersCount = Teachers.Count();
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
        
        [HttpPost("GetLecturesCount")]
        public async Task<IActionResult> GetLecturesCount()
        {
            try
            {
                var Lectures = await _unitOfWork.LectureRepo.GetAll();
                int LecturesCount = Lectures.Count();
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
    }
}
