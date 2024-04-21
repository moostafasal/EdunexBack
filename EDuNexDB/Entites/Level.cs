using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class Level:BaseEntity
    {
        [Required]
        public string LevelName { get; set; }
        public ICollection<ApplicationUser> Students { get; set; }
    }

}
