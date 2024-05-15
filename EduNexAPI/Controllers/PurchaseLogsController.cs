using EduNexBL.IRepository;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseLogsController : ControllerBase
    {
        private readonly IEduNexPurchaseLogs _eduNexPurchaseLogsRepo;

        public PurchaseLogsController(IEduNexPurchaseLogs eduNexPurchaseLogsRepo)
        {
            _eduNexPurchaseLogsRepo = eduNexPurchaseLogsRepo;
        }


        [HttpGet("GetAllLogs")]
        public async Task<ActionResult<IEnumerable<EduNexPurchaseLogs>>> GetAllLogs()
        {
            try
            {
                var logs = await _eduNexPurchaseLogsRepo.GetAllLogs();
                if (logs == null)
                {
                    return NotFound();
                }
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<EduNexPurchaseLogs>> GetById(string id)
        {
            try
            {
                var log = await _eduNexPurchaseLogsRepo.GetById(id);
                if (log == null)
                {
                    return NotFound();
                }
                return Ok(log);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllLogsByCourseId/{courseId}")]
        public async Task<ActionResult<IEnumerable<EduNexPurchaseLogs>>> GetAllLogsByCourseId(int courseId)
        {
            try
            {
                var logs = await _eduNexPurchaseLogsRepo.GetAllLogsByCourseId(courseId);
                if (logs == null)
                {
                    return NotFound();
                }
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllLogsByReceiverId/{teacherId}")]
        public async Task<ActionResult<IEnumerable<EduNexPurchaseLogs>>> GetAllLogsByReceiverId(string teacherId)
        {
            try
            {
                var logs = await _eduNexPurchaseLogsRepo.GetAllLogsByReceiverId(teacherId);
                if (logs == null)
                {
                    return NotFound();
                }
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllLogsBySenderId/{studentId}")]
        public async Task<ActionResult<IEnumerable<EduNexPurchaseLogs>>> GetAllLogsBySenderId(string studentId)
        {
            try
            {
                var logs = await _eduNexPurchaseLogsRepo.GetAllLogsBySenderId(studentId);
                if (logs == null)
                {
                    return NotFound();
                }
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("CalculateBalance")]
        public IActionResult CalculateBalance()
        {
            var amountsum = _eduNexPurchaseLogsRepo.CalculateBalance();
            var result = new
            {
                resultData = amountsum
            };
            return Ok(result);
        }
    }
}
