using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.ExamintionDtos
{
    public class SubmittedQuestionDto
    {
        [Required]
        public int QuestionId { get; set; } // Identifier of the question
        public List<int?> SelectedAnswersIds { get; set; } // Identifier of the selected answer (if applicable)

    }
}
