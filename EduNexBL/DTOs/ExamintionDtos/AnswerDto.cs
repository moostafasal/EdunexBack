using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.ExamintionDtos
{
    public class AnswerDto
    {
        public int AnswerId { get; set; }
        public string Header { get; set; } = null!;
        public bool IsCorrect { get; set; }
    }
}
