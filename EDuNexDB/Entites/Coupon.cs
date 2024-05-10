using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
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
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
