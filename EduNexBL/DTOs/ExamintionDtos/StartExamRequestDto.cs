using System.ComponentModel.DataAnnotations;

namespace EduNexBL.DTOs.ExamintionDtos
{
    public class StartExamRequestDto
    {
        [Required]
        public string StudentId { get; set; }
    }
}