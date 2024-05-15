
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
        public virtual Student Student { get; set; }
        public int ExamId { get; set; }
        public virtual Exam Exam { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? EndTime { get; set; }
        public int? Score { get; set; }
    }
}
