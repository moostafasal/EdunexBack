using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.CourseDTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = null!;

        public string Thumbnail { get; set; } = null!;

        public decimal Price { get; set; }

        public string SubjectName { get; set; } = null!;

        public string TeacherName { get; set; } = null!;
        public string ProfilePhoto { get; set; } = null!;
        public string LevelName { get; set; } = null!;
        public List<LectureDto>? LectureList { get; set;} = new List<LectureDto>();

    }
}
