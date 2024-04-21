using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class Transaction:BaseEntity
    {
        

        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet? Wallet { get; set; }

        [Required]
        public string TransactionType { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }
    }
}
