using AutoMapper;
using EduNexBL.DTOs.CourseDTOs;
using EduNexBL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduNexAPI.Controllers
{
    [Route("api/courses/{courseId}/lectures/{lectureId}/videos")]
    [ApiController]
    public class VideosController : ControllerBase
    {
     
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 
        public VideosController(IUnitOfWork unitOfWork,IMapper mapper)
        {
           _unitOfWork =   unitOfWork;
            _mapper = mapper;
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
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<VideosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VideosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
