using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.PaymentDTOs
{
    public class TransactionDTO
    {
        public int WalletId { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string TransactionDate { get; set; }
    }
}
