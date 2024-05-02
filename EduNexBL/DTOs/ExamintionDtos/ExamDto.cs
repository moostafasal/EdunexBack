using EduNexBL.ENums;
using EduNexBL.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.ExamintionDtos
{
    public class ExamDto
    {
        public int? ExamId { get; set; }

        [Required]
        public string Title { get; set; } = null!;
        [Required]
        [DataType(DataType.DateTime)]
        [StartTimeAfterNow]
        public DateTime StartDateTime { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [EndTimeAfterStartTime]
        public DateTime EndDateTime { get; set; }
        [Required]
        [DurationWithinTimeRange]
        public int Duration { get; set; }
        [Required]
        [EnumDataType(typeof(AssessmentType))]
        public string Type { get; set; } = null!;
        public int LectureId { get; set; } 
        public IEnumerable<QuestionDto>? Questions { get; set; }
    }
}
