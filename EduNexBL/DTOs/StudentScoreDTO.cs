using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs
{
    public class StudentScoreDTO
    {
        public string? StudentId { get; set; }
        public string StudentName { get; set; }
        public int? ExamId { get; set; }
        public int Score { get; set; }
    }
}
