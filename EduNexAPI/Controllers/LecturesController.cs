using System.Security.Claims;
using AutoMapper;
using EduNexBL.DTOs.CourseDTOs;
using EduNexBL.UnitOfWork;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduNexAPI.Controllers
{
    [Route("api/courses/{courseId}/lectures")]
    [ApiController]

    public class LecturesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LecturesController> logger;

        public LecturesController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, ILogger<LecturesController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            this.logger = logger;
        }
        //GET: api/courses/{courseId/lectures

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LectureDto>>> GetLecturesByCourseId(int courseId)
        {
            var course = _unitOfWork.CourseRepo.GetById(courseId);
            if (course == null) { return NotFound("Course Not Found"); }
            var lectures = await _unitOfWork.LectureRepo.GetLecturesByCourseId(courseId);

            if (lectures == null)
            {
                return NotFound("lectures not found");
            }

            List<LectureDto> lectureDtos = new List<LectureDto>();
            foreach (var lecture in lectures)
            {
                var lecDto = _unitOfWork.CourseRepo.MapLectureToLectureDTO(lecture);
                lectureDtos.Add(lecDto);
            }
            return Ok(lectureDtos);
        }


        // GET: api/courses/{courseId}/lectures/{lectureId}
        //[HttpGet("{lectureId}")]

        //public async Task<IActionResult> GetLecture(int lectureId)
        //{

        //    var lecture = await _unitOfWork.LectureRepo.GetFullLectureById(lectureId);

        //    if (lecture == null)
        //    {
        //        return NotFound("Lecture not found");
        //    }

        //    var lectureDto = _unitOfWork.CourseRepo.MapLectureToLectureDTO(lecture);
        //    return Ok(lectureDto);
        //}

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student,Teacher,Admin")]




        [HttpGet("{lectureId}")]
        public async Task<IActionResult> GetLecture(string userId, int lectureId)
        {
            // Check the role of the user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            // Retrieve the lecture
            var lecture = await _unitOfWork.LectureRepo.GetFullLectureById(lectureId);
            if (lecture == null)
            {
                return NotFound("Lecture not found");
            }
            logger.LogInformation("User Roles: {Roles}", string.Join(", ", userRoles));


            // If the user is an admin, allow access to the lecture
            if (userRoles.Contains("Admin"))
            {
                var lectureDto = _unitOfWork.CourseRepo.MapLectureToLectureDTO(lecture);
                return Ok(lectureDto);
            }

            // If the user is a student, check if they are enrolled in the course related to the lecture
            if (userRoles.Contains("Student"))
            {
                var isEnrolled = await _unitOfWork.CourseRepo.IsStudentEnrolledInCourse(userId, lecture.CourseId);
                if (!isEnrolled)
                {
                    return StatusCode(403, "Student is not enrolled in the course"); // Return 403 Forbidden with error message
                }
                var lectureDto = _unitOfWork.CourseRepo.MapLectureToLectureDTO(lecture);
                return Ok(lectureDto);

            }

            // If the user is a teacher, check if they are related to the course related to the lecture
            if (userRoles.Contains("Teacher"))
            {
                var isRelated = await _unitOfWork.CourseRepo.IsTeacherRelatedToCourse(userId, lecture.CourseId);
                if (!isRelated)
                {
                    return StatusCode(403, "Teacher is not related to the course"); // Return 403 Forbidden with error message
                }
                var lectureDto = _unitOfWork.CourseRepo.MapLectureToLectureDTO(lecture);
                return Ok(lectureDto);
            }

            // If the user's role is not recognized, deny access
            logger.LogInformation("Calling IsStudentEnrolledInCourse for User ID: {UserId} and Course ID: {CourseId}", userId, lecture.CourseId);

            return StatusCode(403);
        }





        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student,Teacher,Admin")]

        [HttpPost]
        public async Task<IActionResult> CreateLecture(LectureDto lecture, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            // Check if the user is a teacher or admin
            if (userRoles.Contains("Teacher") || userRoles.Contains("Admin"))
            {
                var course = await _unitOfWork.CourseRepo.GetCourseById(lecture.CourseId);
                if (course == null)
                {
                    return NotFound("Course not found");
                }

                var lectureToAdd = _mapper.Map<Lecture>(lecture);
                await _unitOfWork.LectureRepo.Add(lectureToAdd);

                return Ok();
            }

            // If the user's role is not recognized, deny access
            return Forbid("Role not recognized");
        }


        // PUT: api/courses/{courseId}/lectures/{lectureId}
        [HttpPut("{lectureId}")]
        public async Task<IActionResult> UpdateLecture(int lectureId, [FromBody] LectureDto updatedLectureData, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (lectureId != updatedLectureData.Id)
            {
                return BadRequest("Invalid lecture ID in the request body.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            // Check if the user is a teacher or admin
            if (userRoles.Contains("Teacher") || userRoles.Contains("Admin"))
            {
                // If the user is a teacher, check if they are related to the course
                if (userRoles.Contains("Teacher"))
                {
                    var lecture = await _unitOfWork.LectureRepo.GetById(lectureId);
                    if (lecture == null)
                    {
                        return NotFound("Lecture not found");
                    }

                    var isRelated = await _unitOfWork.CourseRepo.IsTeacherRelatedToCourse(userId, lecture.CourseId);
                    if (!isRelated)
                    {
                        return StatusCode(403, "Teacher is not related to the course"); // Return 403 Forbidden with error message
                    }
                }

                var lectureToUpdate = await _unitOfWork.LectureRepo.GetById(lectureId);
                if (lectureToUpdate == null)
                {
                    return NotFound("Lecture not found");
                }

                // Update the lecture data
                lectureToUpdate.LectureTitle = updatedLectureData.LectureTitle;
                lectureToUpdate.Price = updatedLectureData.Price;

                await _unitOfWork.LectureRepo.Update(lectureToUpdate);

                return Ok();
            }

            // If the user's role is not recognized, deny access
            return Forbid("Role not recognized");
        }


        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student,Teacher,Admin")]


        // DELETE: api/courses/{courseId}/lectures/{lectureId}
        [HttpDelete("{lectureId}")]
        public async Task<IActionResult> DeleteLecture(int lectureId, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var lecture = await _unitOfWork.LectureRepo.GetById(lectureId);
            if (lecture == null)
            {
                return NotFound("Lecture not found");
            }
            // Check if the user is a teacher or admin
            if (userRoles.Contains("Teacher") || userRoles.Contains("Admin"))
            {
                // If the user is a teacher, check if they are related to the course
                if (userRoles.Contains("Teacher"))
                {
                    var isRelated = await _unitOfWork.CourseRepo.IsTeacherRelatedToCourse(userId, lecture.CourseId);
                    if (!isRelated)
                    {
                        return StatusCode(403, "Teacher is not related to the course"); // Return 403 Forbidden with error message
                    }
                }



                // Delete the lecture
                await _unitOfWork.LectureRepo.Delete(lecture);
                return Ok($"{lectureId} Deleted");
            }

            // If the user's role is not recognized, deny access
            return Forbid("Role not recognized");
        }



    }
}