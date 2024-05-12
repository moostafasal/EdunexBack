using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.CourseDTOs
{
    public class TeacherCoursesViewDTO
    {
        public int Id { get; set; }
        public string CourseName { get; set; } = null!;

        public string Thumbnail { get; set; } = null!;

        public decimal Price { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = null!;
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }


    }
}
