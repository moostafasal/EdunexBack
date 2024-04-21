using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class Question:BaseEntity
    {



        [Required]
        public string Header { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;
        [Required]
        public int Points { get; set; }
        public ICollection<Answer>? Answers { get; set; } = null!;
        public int ExamId { get; set; }
        public Exam? Exam { get; set; }
    }
}
