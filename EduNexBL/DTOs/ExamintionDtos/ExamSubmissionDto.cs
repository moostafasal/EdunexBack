using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.ExamintionDtos
{
    public class ExamSubmissionDto
    {
        [Required]
        public string StudentId { get; set; }

        [Required]
        public List<SubmittedQuestionDto> Answers { get; set; } = null!;
    }
}
