using AutoMapper;
using EduNexBL.DTOs.CourseDTOs;
using EduNexBL.UnitOfWork;
using EduNexDB.Entites;
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
        public LecturesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
        [HttpGet("{lectureId}")]
        public async Task<IActionResult> GetLecture(int lectureId)
        {
            var lecture = await _unitOfWork.LectureRepo.GetById(lectureId);

            if (lecture == null)
            {
                return NotFound("Lecture not found");
            }

            var lectureDto = _unitOfWork.CourseRepo.MapLectureToLectureDTO(lecture);
            return Ok(lectureDto);
        }

        // POST: api/courses/{courseId}/lectures
        [HttpPost]
        public async Task<IActionResult> CreateLecture(LectureDto lecture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _unitOfWork.CourseRepo.GetCourseById(lecture.CourseId);
            if (course == null) { return NotFound(); }

            var lectureToAdd = _mapper.Map<Lecture>(lecture);
            await _unitOfWork.LectureRepo.Add(lectureToAdd);

            return CreatedAtAction(nameof(GetLecture), new { id = lecture.Id }, lecture);
        }

        // PUT: api/courses/{courseId}/lectures/{lectureId}
        [HttpPut("{lectureId}")]
        public async Task<IActionResult> UpdateLecture(int id,[FromBody] LectureDto updatedLectureData)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            if (id != updatedLectureData.Id) return BadRequest(); 

            var lectureToUpdate = await _unitOfWork.LectureRepo.GetById(id); 
            if (lectureToUpdate == null) { return NotFound(); }

            lectureToUpdate.LectureTitle = updatedLectureData.LectureTitle; 
            lectureToUpdate.Price = updatedLectureData.Price;

            await _unitOfWork.LectureRepo.Update(lectureToUpdate);

            return Ok();
        }

        // DELETE: api/courses/{courseId}/lectures/{lectureId}
        [HttpDelete("{lectureId}")]
        public async Task<IActionResult> DeleteLecture(int lectureId)
        {
            var lec = await _unitOfWork.LectureRepo.GetById(lectureId);
            if (lec == null) { return NotFound(); }
            await _unitOfWork.LectureRepo.Delete(lec);
            return Ok($"{lectureId} Deleted");
        }

    }
}