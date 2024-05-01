using AuthenticationMechanism.Services;
using AutoMapper;
using CloudinaryDotNet;
using EduNexBL.DTOs.CourseDTOs;
using EduNexBL.DTOs.ExamintionDtos;
using EduNexBL.IRepository;
using EduNexBL.UnitOfWork;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduNexAPI.Controllers
{

    [Route("api/courses/{courseId}/lectures/{lectureId}/attachments")]
    //[Route("api/courses/videos")]

    [ApiController]
    public class AttachmentsController : ControllerBase
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFiles _cloudinary;

        public AttachmentsController(IUnitOfWork unitOfWork,IMapper mapper , IFiles files)
        {
            _unitOfWork =   unitOfWork;
            _mapper = mapper;
            _cloudinary = files;
        }

        [HttpGet]
        public async  Task<IActionResult> Get()
        {
            var attachments =  await _unitOfWork.AttachmentRepo.GetAll();
            List<AttachmentDto> attachmentsDto = new(); 
            foreach (var a in attachments)
            {
                attachmentsDto.Add(_mapper.Map<AttachmentDto>(a));
            }
            return Ok(attachmentsDto); 

        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
           var attachment = _unitOfWork.AttachmentRepo.GetById(id);
           if (attachment == null)
           {
                return NotFound(); 
           }

           return Ok(_mapper.Map<AttachmentDto>(attachment)); 
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm]AttachmentAddDto attachmentDto)
        {
            //name file 
            var filePath= await _cloudinary.UploadRawAsync(attachmentDto.File);
            var attachment = new AttachmentFile
            {
                AttachmentPath = filePath,
                AttachmentTitle = attachmentDto.AttachmentTitle,
                LectureId = attachmentDto.LectureId

            };
            await _unitOfWork.AttachmentRepo.Add(attachment);

            return Ok("Attachment Uploaded and Added");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(AttachmentDto attachmentDto)
        {
            var existingAttachment = await _unitOfWork.AttachmentRepo.GetById(attachmentDto.Id); 
            if (existingAttachment == null) { return NotFound(); }
            existingAttachment.AttachmentTitle = attachmentDto.AttachmentTitle;
            existingAttachment.AttachmentPath = attachmentDto.AttachmentPath;

            await _unitOfWork.AttachmentRepo.Update(existingAttachment); 
            return Ok(existingAttachment);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingAttachment = await _unitOfWork.AttachmentRepo.GetById(id);
            if (existingAttachment == null) { return NotFound(); }
            
            await _unitOfWork.AttachmentRepo.Delete(existingAttachment);
            return Ok("Deleted");
        }
    }
}
