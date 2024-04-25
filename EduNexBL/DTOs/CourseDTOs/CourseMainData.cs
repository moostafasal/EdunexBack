using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.CourseDTOs
{
    public class CourseMainData
    {
        public string CourseName { get; set; } = null!;

        public string Thumbnail { get; set; } = null!;

        public string CourseType { get; set; } = null!;

        public decimal Price { get; set; }

        public string SubjectName { get; set; } = null!; 

        public string TeacherName { get; set; } = null!;
        public string ProfilePhoto { get; set; } = null!;
        public string LevelName { get; set; } = null!;


    }
}
