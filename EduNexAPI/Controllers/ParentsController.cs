using EduNexBL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ParentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> GetStudentSummary(string nationalId)
        {
            var studnetId = await _unitOfWork.StudentRepo.GetStudentIdByNationalId(nationalId);
            if (studnetId == null) return NotFound();
            var student = await _unitOfWork.StudentRepo.GetById(studnetId);
            var studnetName= $"{student.FirstName} {student.LastName}";

            var studentCourses = await _unitOfWork.CourseRepo.CoursesEnrolledByStudent(studnetId);
            var studentExams = await _unitOfWork.StudentRepo.GetExamsSubmissions(studnetId);
            var summary = new
            {
                studnetName,
                studentCourses,
                studentExams
            };
            return Ok(summary); 
        }
    }
}
