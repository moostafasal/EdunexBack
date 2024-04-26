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

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExamsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            return CreatedAtAction(nameof(Get), new { id = createdExamDto.ExamId }, createdExamDto);
        }

        // PUT api/<ExamsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ExamDto examDto)
        {
            if (id != examDto.ExamId)
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
        public async Task<IActionResult> StartExam(int id, [FromBody] StartExamRequestDto request)
        {
            // Validate the request model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exam = await _unitOfWork.ExamRepo.GetById(id);
            var student = await _unitOfWork.StudentRepo.GetById(request.StudentId);
            if (exam == null || student == null) { return NotFound(); }

            // Call the service method to start the exam
            var result = await _unitOfWork.ExamRepo.StartExam(request.StudentId, id);
            ////////////attention  

            switch (result)
            {
                case ExamStartResult.Success:
                    return Ok("Exam started successfully.");
                default:
                    return BadRequest(result);
            }
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitExam(int id, [FromBody] ExamSubmissionDto submission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _unitOfWork.ExamRepo.SubmitExam(id, submission);

            if (response.SubmitResult == ExamSubmitResult.Success)
                return Ok(response);
            else
                return BadRequest(response.SubmitResult);

        }

        [HttpGet("{id}/result")]
        public async Task<IActionResult> GetExamResult(int id, [FromBody] string studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _unitOfWork.ExamRepo.GetExamSubmitResultWithDetails(id, studentId);


            return Ok(response); 
            //if (response.SubmitResult == ExamSubmitResult.Success)
            //    return Ok(response);
            //else
            //    return BadRequest(response.SubmitResult);

        }
    }
}