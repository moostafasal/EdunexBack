using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.PaymentDTOs
{
    internal class WalletDTO
    {
        public decimal Balance { get; set; }
        public int OwnerId { get; set; }
    }
}
