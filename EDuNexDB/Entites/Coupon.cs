using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public enum CouponType
    {
        Charge_Coupon,
        Discount_Coupon
    }
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CouponCode { get; set; }

        [Required]
        [DataType("decimal(18,2)")]
        public decimal Value { get; set; }

        [Required]
        public int NumberOfUses { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(3);

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public CouponType CouponType { get; set; }

    }
}
