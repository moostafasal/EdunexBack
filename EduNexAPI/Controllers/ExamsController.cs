using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using EduNexDB.Entites;
using EduNexBL.IRepository;
using EduNexBL.ENums;
using EduNexBL.Repository;
using EduNexBL.UnitOfWork;
using EduNexBL.DTOs.ExamintionDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExamsController(IUnitOfWork unitOfWork, IMapper mapper,UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: api/<ExamsController>

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]

        public async Task<ActionResult<IEnumerable<ExamDto>>> Get()
        {
            var exams = await _unitOfWork.ExamRepo.GetAllExamsWithQuestions();
            var examDtos = _mapper.Map<IEnumerable<Exam>, IEnumerable<ExamDto>>(exams);
            return Ok(examDtos);
        }

        // GET api/<ExamsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExamDto>> Get(int id)
        {
            var exam = await _unitOfWork.ExamRepo.GetExamByIdWithQuestionsAndAnswers(id);
            if (exam == null)
            {
                return NotFound();
            }
            var examDto = _mapper.Map<ExamDto>(exam);
            return Ok(examDto);
        }

        // POST api/<ExamsController>
        [HttpPost]
        public async Task<ActionResult<ExamDto>> Post(ExamDto examDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exam = _mapper.Map<Exam>(examDto);
            await _unitOfWork.ExamRepo.Add(exam);

            var createdExamDto = _mapper.Map<ExamDto>(exam);
            return CreatedAtAction(nameof(Get), new { id = createdExamDto.Id }, createdExamDto);
        }

        // PUT api/<ExamsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ExamDto examDto)
        {
            if (id != examDto.Id)
            {
                return BadRequest();
            }

            var existingExam = await _unitOfWork.ExamRepo.GetById(id);
            if (existingExam == null)
            {
                return NotFound();
            }

            _mapper.Map(examDto, existingExam);
            await _unitOfWork.ExamRepo.Update(existingExam);

            return NoContent();
        }

        // DELETE api/<ExamsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exam = await _unitOfWork.ExamRepo.GetById(id);
            if (exam == null)
            {
                return NotFound();
            }

            await _unitOfWork.ExamRepo.Delete(exam);

            return NoContent();
        }


        [HttpPost("{id}/start")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student")]

        public async Task<IActionResult> StartExam(int id, [FromBody] StartExamRequestDto request)
        {

            // Validate the request model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exam = await _unitOfWork.ExamRepo.GetById(id);
            var student = await _unitOfWork.StudentRepo.GetById(request.StudentId);
            if (exam == null || student == null)
            {
                return NotFound();
            }

            var courseID = await _unitOfWork.ExamRepo.GetCourseIdOfExam(id);

            var isEnrolled = await _unitOfWork.CourseRepo.IsStudentEnrolledInCourse(request.StudentId, courseID);
            if (!isEnrolled)
            {
                return StatusCode(403, "Student is not enrolled in the course"); // Return 403 Forbidden with error message
            }
            // Call the service method to start the exam
            var result = await _unitOfWork.ExamRepo.StartExam(request.StudentId, id);
            ////////////attention  

            // Switch statement to handle different results from starting the exam
            return result switch
            {
                ExamStartResult.Success => Ok(),
                ExamStartResult.NotFound => NotFound("Exam or student not found."),
                ExamStartResult.NotAvailable => BadRequest("Exam is not available."),
                ExamStartResult.InvalidDuration => BadRequest("Invalid exam duration."),
                ExamStartResult.AlreadyStarted => Ok(),
                _ => BadRequest("Unknown error occurred."),// This may not happen in practice, but it's good to have a default case for completeness.
            };
        }


        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitExam(int id, [FromBody] ExamSubmissionDto submission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _unitOfWork.ExamRepo.SubmitExam(id, submission);

            // Return text response based on the ExamSubmitResult
            switch (response.SubmitResult)
            {
                case ExamSubmitResult.Success:
                    return Ok("Exam submission successful");
                case ExamSubmitResult.NotFound:
                    return BadRequest("Exam not found");
                case ExamSubmitResult.NotAvailable:
                    return BadRequest("Exam not available");
                case ExamSubmitResult.NotStarted:
                    return BadRequest("Exam not started");
                case ExamSubmitResult.ExamNotEnded:
                    return BadRequest("Exam not ended");
                default:
                    return BadRequest("Unknown error occurred");
            }
        }

        [HttpGet("{examId}/submission/{studentId}")]
        public async Task<IActionResult> GetExamStudentResult(int examId, string studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _unitOfWork.ExamRepo.GetExamSubmitResultWithDetails(examId, studentId);

            // Return text response based on the ExamSubmitResult
            return Ok(response);
        }



        [HttpGet("{id}/result/{studentId}")]
        public async Task<IActionResult> GetExamResult(int id,string studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _unitOfWork.ExamRepo.GetExamSubmitResultWithDetails(id, studentId);

            // Return text response based on the ExamSubmitResult
            switch (response.SubmitResult)
            {
                case ExamSubmitResult.Success:
                    return Ok("Exam submission successful");
                case ExamSubmitResult.NotFound:
                    return BadRequest("Exam not found");
                case ExamSubmitResult.NotAvailable:
                    return BadRequest("Exam not available");
                case ExamSubmitResult.NotStarted:
                    return BadRequest("Exam not started");
                case ExamSubmitResult.ExamNotEnded:
                    return BadRequest("Exam not ended");
                default:
                    return BadRequest("Unknown error occurred");
            }
        }


        [HttpGet("{id}/getinfo/{studentId}")]
        public async Task<IActionResult> GetStudentExamInfo(string studentId, int id)
        {
            var info = await _unitOfWork.ExamRepo.GetStudentExamInfo(studentId, id);
            if (info != null) return Ok(info);
            else return NotFound();
        }

    }
}