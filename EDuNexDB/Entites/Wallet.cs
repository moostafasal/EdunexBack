using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }

        [Required]
        [DataType("integer")]
        public decimal Balance { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public string OwnerType { get; set; }

    }
}
