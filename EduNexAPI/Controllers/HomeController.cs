using EduNexBL.UnitOfWork;
using EduNexDB.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
