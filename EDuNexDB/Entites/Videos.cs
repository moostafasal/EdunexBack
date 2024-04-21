using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class Video:BaseEntity
    {
      

        [Required]
        public string VideoTitle { get; set; }

        [Required]
        public string VideoPath { get; set; }

        [ForeignKey("Lecture")]
        public int LectureId { get; set; }
        public Lecture? Lecture { get; set; }
    }
}
