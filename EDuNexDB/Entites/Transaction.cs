using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet? Wallet { get; set; }
        [Required]
        public string TransactionType { get; set; }
        [Required]
        [DataType("integer")]
        public decimal Amount { get; set; }
        [Required]
        public string TransactionDate { get; set; }
    }
}
