
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public class StudentExam
    {
        public string StudentId { get; set; }
        public Student Student { get; set; } = new ();
        public int ExamId { get; set; }
        public Exam Exam { get; set; } = new Exam();

        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndTime { get; set; }

        public int? Score { get; set; }
    }
}
