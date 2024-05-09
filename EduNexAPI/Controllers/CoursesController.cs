using EduNexBL.DTOs.CourseDTOs;
using EduNexBL.DTOs.ExamintionDtos;
using EduNexBL.ENums;
using EduNexBL.UnitOfWork;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        public IUnitOfWork _unitOfWork { get; set; }
        public CoursesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }

        // GET: api/<CoursesController>
        [HttpGet]
        public async Task<IEnumerable<CourseMainData>> Get()
        {
            return await _unitOfWork.CourseRepo.GetAllCoursesMainData();
        }

        // GET api/<CoursesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> Get(int id)
        {
            var Course = await _unitOfWork.CourseRepo.GetCourseById(id);
            if (Course == null)
            {
                return NotFound();
            }

            return Ok(Course);
        }

        // POST api/<CoursesController>
        [HttpPost]
        public async Task<ActionResult<Course>> Post(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _unitOfWork.CourseRepo.Add(course);
            return CreatedAtAction(nameof(Get), new { id = course.Id }, course);

        }

        // PUT api/<CoursesController>/5

        // PUT api/<ExamsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            var existingCourse = await _unitOfWork.CourseRepo.GetById(id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            
            await _unitOfWork.CourseRepo.Update(course);

            return NoContent();
        }

        // DELETE api/<CoursesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _unitOfWork.CourseRepo.GetById(id);
            if (course == null)
            {
                return NotFound();
            }

            await _unitOfWork.CourseRepo.Delete(course);

            return NoContent();
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollStudentInCourse(EnrollmentRequestDto enrollmentRequestDto)
        {
            var result = await _unitOfWork.CourseRepo.EnrollStudentInCourse(enrollmentRequestDto.StudentId, enrollmentRequestDto.CourseId);
            return result switch
            {
                EnrollmentResult.Success => Ok(),
                EnrollmentResult.StudentNotFound => NotFound("Student not found."),
                EnrollmentResult.CourseNotFound => NotFound("Course not found."),
                EnrollmentResult.AlreadyEnrolled => BadRequest("Student is already enrolled in the course."),
                _ => StatusCode(500, "An error occurred while processing the enrollment.")
            };
        }

        [HttpGet("checkenrollment")]
        public async Task<IActionResult> CheckEnrollment([FromQuery]EnrollmentRequestDto enrollmentDto)
        {
            var isEnrolled = await _unitOfWork.CourseRepo.IsStudentEnrolledInCourse(enrollmentDto.StudentId, enrollmentDto.CourseId);
            return Ok(isEnrolled);
        }
        [HttpGet("GetCoursesEnrolledByStudent")]
        public async Task<IActionResult> GetCoursesByStudent(string studentId)
        {
            var student = await _unitOfWork.StudentRepo.GetById(studentId);
            if (student == null)
            {
                return NotFound("student not found");
            }
            return Ok( await _unitOfWork.CourseRepo.CoursesEnrolledByStudent(studentId));
        }

    }
}
