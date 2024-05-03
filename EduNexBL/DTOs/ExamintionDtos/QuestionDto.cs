using EduNexBL.ENums;
using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.ExamintionDtos
{
    public class QuestionDto
    {
        public int? Id { get; set; }
        [Required]
        public string Header { get; set; } = null!;
        [Required]
        [EnumDataType(typeof(QuestionType))]
        public string Type { get; set; } = null!;
        [Required]
        public int Points { get; set; }
        public IEnumerable<AnswerDto> Answers { get; set; } = null!;
    }
}
