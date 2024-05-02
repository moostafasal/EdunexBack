using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.ExamintionDtos
{
    public class AnswerDto
    {
        public int? AnswerId { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
    }
}
