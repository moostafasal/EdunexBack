using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class Exam:BaseEntity
    {

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        [Required]
        public int Duration { get; set; }

        [Required]
        public string Type { get; set; } = null!;
        public int LectureId { get; set; }
        public virtual Lecture? Lecture { get; set; }
        public ICollection<Question>? Questions { get; set; }
        public ICollection<StudentExam>? StudentExams { get; set; }
    }
}
