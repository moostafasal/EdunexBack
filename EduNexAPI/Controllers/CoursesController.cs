using EduNexBL.DTOs.CourseDTOs;
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
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CoursesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CoursesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CoursesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
