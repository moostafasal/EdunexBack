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

    //[Route("api/courses/{courseId}/lectures/{lectureId}/videos")]
    [Route("api/courses/videos")]

    [ApiController]
    public class VideosController : ControllerBase
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFiles _cloudinary;

        public VideosController(IUnitOfWork unitOfWork,IMapper mapper , IFiles files)
        {
           _unitOfWork =   unitOfWork;
            _mapper = mapper;
            _cloudinary = files;
        }

        [HttpGet]
        public async  Task<IActionResult> Get()
        {
            var videos =  await _unitOfWork.VideoRepo.GetAll();
            List<VideoDTO> videosDTO = new(); 
            foreach (var v in videos)
            {
                videosDTO.Add(_mapper.Map<VideoDTO>(v));
            }
            return Ok(videosDTO); 

        }

        // GET api/<VideosController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
           var video = _unitOfWork.VideoRepo.GetById(id);
           if (video  == null)
           {
                return NotFound(); 
           }

           return Ok(_mapper.Map<VideoDTO>(video)); 
        }

        // POST api/<VideosController>
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file , string Name)
        {
            //name file 
          var filePath= await _cloudinary.UploadVideoAsync(file);
            var video = new Video
            {
                VideoPath = filePath,
                VideoTitle=Name
            };
            await _unitOfWork.VideoRepo.Add(video);

           var createdVideo=_mapper.Map<VideoDTO>(video);


            return CreatedAtAction(nameof(Get), createdVideo);
        }

        // PUT api/<VideosController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(VideoDTO videoDTO)
        {
            var existingVideo = await _unitOfWork.VideoRepo.GetById(videoDTO.id); 
            if (existingVideo == null) { return NotFound(); }
            existingVideo.VideoTitle = videoDTO.VideoTitle;
            existingVideo.VideoPath = videoDTO.VideoPath;

            await _unitOfWork.VideoRepo.Update(existingVideo); 
            return Ok(existingVideo);
        }

        // DELETE api/<VideosController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingVideo = await _unitOfWork.VideoRepo.GetById(id);
            if (existingVideo == null) { return NotFound(); }
            
            await _unitOfWork.VideoRepo.Delete(existingVideo);
            return Ok("Deleted");
        }
    }
}
