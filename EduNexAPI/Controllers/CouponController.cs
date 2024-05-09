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
        public ActionResult<Coupon> GenerateCoupon(decimal value, int numberOfUses)
        {
            var coupon = _couponService.GenerateCoupon(value, numberOfUses);
            return Ok(coupon);
        }

        [HttpPost("consume")]
        public ActionResult<bool> ConsumeCoupon(string couponCode, string ownerId, string ownerType)
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
    }
}
