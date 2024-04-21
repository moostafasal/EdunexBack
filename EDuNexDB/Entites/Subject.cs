using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class Subject:BaseEntity
    {
        

        [Required]
        public string SubjectName { get; set; }

        [ForeignKey("Level")]
        public int LevelId { get; set; }
        public Level? Level { get; set; }
    }

}
