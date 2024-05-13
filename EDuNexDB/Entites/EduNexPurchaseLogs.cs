using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class EduNexPurchaseLogs 
    {
        [Key]
        public string Id { get; set; } 

        [Required]
        public int CourseId { get; set; }

        [Required]
        public string SenderId { get; set; } // ID of the sender (e.g., student)

        [Required]
        [ForeignKey("Teacher")]
        public string ReceiverId { get; set; } // ID of the receiver (e.g., teacher or platform)

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AmountReceived { get; set; }

        [Required]
        public bool IsCouponUsed { get; set; } // Indicates whether the coupon is used or not

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CouponsValue { get; set; } // Value of coupons used

        [Required]
        public DateTime DateAdded { get; set; }

        public virtual Teacher? Teacher { get; set; }
    }
}
