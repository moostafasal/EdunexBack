using AuthenticationMechanism.Services;
using EduNexBL.DTOs;
using EduNexBL.DTOs.CourseDTOs;
using EduNexBL.DTOs.ExamintionDtos;
using EduNexBL.DTOs.SubjectDTOs;
using EduNexBL.ENums;
using EduNexBL.UnitOfWork;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        public IUnitOfWork _unitOfWork { get; set; }
        public IFiles _files { get; }
        public UserManager<ApplicationUser> _userManager { get; }

        public CoursesController(IUnitOfWork unitOfWork, IFiles files,UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _files = files;
            _userManager = userManager;
        }

        // GET: api/<CoursesController>
        [HttpGet]
        public async Task<IEnumerable<CourseMainData>> Get()
        {
            return await _unitOfWork.CourseRepo.GetAllCoursesMainData();
        }

        [HttpGet("get-All-Subject")]
        public async Task<IEnumerable<SubjectRDTO>> GetSubjects()
        {
            return await _unitOfWork.CourseRepo.Getsubject();
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
        public async Task<ActionResult> Post([FromForm] AddUpdateCourseDTO course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var filePath = await _files.UploadVideoAsync(course.Thumbnail);
            var createdCourse = new Course
            {
                CourseName = course.CourseName,
                Thumbnail = filePath,
                Price = course.Price,
                SubjectId = course.SubjectId,
                TeacherId = course.TeacherId,
            };
            await _unitOfWork.CourseRepo.Add(createdCourse);
            return Ok();

        }

        // PUT api/<CoursesController>/5

        // PUT api/<ExamsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] AddUpdateCourseDTO course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCourse = await _unitOfWork.CourseRepo.GetById(id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            string filePath = existingCourse.Thumbnail;
            if (course.Thumbnail != null)
            {
                filePath = await _files.UploadVideoAsync(course.Thumbnail);
            }

            existingCourse.CourseName = course.CourseName;
            existingCourse.Thumbnail = filePath;
            existingCourse.Price = course.Price;
            existingCourse.SubjectId = course.SubjectId;
            existingCourse.TeacherId = course.TeacherId;

            await _unitOfWork.CourseRepo.Update(existingCourse);

            return Ok();
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
        public async Task<IActionResult> EnrollStudentInCourse(EnrollmentRequestDto enrollmentRequestDto,[FromQuery] string[]? couponcodes)
        {
            var result = await _unitOfWork.CourseRepo.EnrollStudentInCourse(enrollmentRequestDto.StudentId, enrollmentRequestDto.CourseId, couponcodes);
            return result switch
            {
                EnrollmentResult.Success => Ok(),
                EnrollmentResult.StudentNotFound => NotFound("Student not found."),
                EnrollmentResult.CourseNotFound => NotFound("Course not found."),
                EnrollmentResult.AlreadyEnrolled => BadRequest("Student is already enrolled in the course."),
                EnrollmentResult.InvalidCoupon => BadRequest("Attempting to use an invalid coupon."),
                _ => StatusCode(500, "An error occurred while processing the enrollment.")
            };
        }

        [HttpGet("checkenrollment")]
        public async Task<IActionResult> CheckEnrollment([FromQuery] EnrollmentRequestDto enrollmentDto)
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
            return Ok(await _unitOfWork.CourseRepo.CoursesEnrolledByStudent(studentId));
        }


        [HttpGet("CountStudents")]
        public async Task<IActionResult> CountEnrolledStudentsInCourse([FromQuery]int courseId)
        {
            var course = await _unitOfWork.CourseRepo.GetById(courseId);
            if (course == null) return NotFound();
            int count = await _unitOfWork.CourseRepo.CountEnrolledStudentsInCourse(courseId);
            return Ok(count);
        }

        [HttpGet("CountLectures")]
        public async Task<IActionResult> CountLecturesInCourse([FromQuery] int courseId)
        {
            var course = await _unitOfWork.CourseRepo.GetById(courseId);
            if (course == null) return NotFound();
            int count = await _unitOfWork.CourseRepo.CountCourseLectures(courseId);
            return Ok(count);
        }

        [HttpGet("GetTeacherCourses")]
        public async Task<IActionResult> GetTeacherCourses([FromQuery]string teacherId)
        {
            var teacher = await _userManager.FindByIdAsync(teacherId);
            if (teacher == null ) return NotFound();
            var roles = await _userManager.GetRolesAsync(teacher);
            if (!roles.Contains("Teacher")) return NotFound();

            var courses = await _unitOfWork.CourseRepo.GetTeacherCourses(teacherId);
            return Ok(courses); 
        }



       

        //[HttpGet("GetCoursesOrderedByCreateionDateAscending")]
        //public async Task<IActionResult> GetCoursesOrderedByCreateionDateAscending()
        //{
        //    try
        //    {
        //        var orderedCoursesList = await _unitOfWork.CourseRepo.GetCoursesOrderedByCreateionDateAscending();
        //        if (orderedCoursesList.IsNullOrEmpty())
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            return Ok(orderedCoursesList);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
