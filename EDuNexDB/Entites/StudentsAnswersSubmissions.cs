using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class StudentsAnswersSubmissions:BaseEntity
    {
       
        public string StudentId { get; set; }
        public virtual Student? Student { get; set; }

       
        public int ExamId { get; set; }
        public virtual Exam? Exam { get; set; }

      
        public int QuestionId { get; set; }
        public virtual Question? Question { get; set; }

        [ForeignKey("Answer")]
        public int? AnswerId { get; set; }
        public virtual Answer? Answer { get; set; }
    }
}
