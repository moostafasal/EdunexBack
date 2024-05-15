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

    }
}
