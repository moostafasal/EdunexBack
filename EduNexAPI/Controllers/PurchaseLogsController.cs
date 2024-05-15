using EduNexBL.IRepository;
using EduNexBL.UnitOfWork;
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
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseLogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetAllLogs")]
        public async Task<ActionResult<IEnumerable<EduNexPurchaseLogs>>> GetAllLogs()
        {
            try
            {
                var logs = await _unitOfWork.EduNexPurchaseLogsRepo.GetAllLogs();
                if (logs == null || logs.Count() <= 0)
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
                var log = await _unitOfWork.EduNexPurchaseLogsRepo.GetById(id);
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
                var logs = await _unitOfWork.EduNexPurchaseLogsRepo.GetAllLogsByCourseId(courseId);
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
                var logs = await _unitOfWork.EduNexPurchaseLogsRepo.GetAllLogsByReceiverId(teacherId);
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
                var logs = await _unitOfWork.EduNexPurchaseLogsRepo.GetAllLogsBySenderId(studentId);
                if (logs != null)
                {
                    return Ok(logs);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("CalculateBalance")]
        public IActionResult CalculateBalance()
        {
            var amountsum = _unitOfWork.EduNexPurchaseLogsRepo.CalculateBalance();
            var result = new
            {
                resultData = amountsum
            };
            return Ok(result);
        }
    }
}
