using EduNexBL.Services;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduNexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly EduNexContext _context;
        private readonly CouponService _couponService;

        public CouponController(EduNexContext context, CouponService couponService)
        {
            _context = context;
            _couponService = couponService;
        }

        [HttpPost("generate")]
        public ActionResult<Coupon> GenerateCoupon(decimal value, int numberOfUses, int dayValid, CouponType couponType)
        {
            var coupon = _couponService.GenerateCoupon(value, numberOfUses, dayValid, couponType);
            return Ok(coupon);
        }

        [HttpPost("consume")]
        public ActionResult<bool> ConsumeCoupon(string couponCode, string ownerId, OwnerType ownerType)
        {
            var isConsumed = _couponService.ConsumeCoupon(couponCode, ownerId, ownerType);
            if (isConsumed)
            {
                return Ok(true);
            }
            else
            {
                return NotFound("Coupon not found or all uses have been exhausted. ask help !");
            }
        }

        [HttpGet("GetValidationPeriod")]
        public async Task<IActionResult> GetTimeLeft(string couponCode)
        {
            try
            {
                var timeLeft = await _couponService.GetTimeLeftBeforeExpiration(couponCode);
                if (timeLeft.HasValue)
                {
                    return Ok(timeLeft.Value.ToString(@"d\d\ hh\h\ mm\m\ ss\s"));
                }
                else
                {
                    return NotFound("Coupon not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUsageNumberLeft")]
        public async Task<IActionResult> GetUsageNumberLeft(string couponCode)
        {
            try
            {
                var usageNumberLeft = await _couponService.GetUsageNumberLeft(couponCode);
                if (usageNumberLeft > 0)
                {
                    return Ok(usageNumberLeft);
                }
                else
                {
                    return Ok("This Coupon can't be used anymore.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
