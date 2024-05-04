using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexDB.Entites
{
    public enum CourseType
    {
        [Display(Name = "Literature")]
        Literature,
        [Display(Name = "Scientific")]
        Scientific,
        [Display(Name = "General")]
        General
    }
    public class Course : BaseEntity
    {

        [Required]
        public string CourseName { get; set; } = null!;

        [Required]
        public string Thumbnail { get; set; } = null!;

        [Required]
        [EnumDataType(typeof(CourseType))]
        public CourseType CourseType { get; set; }

        [Required]
        [DataType("integer")]
        public decimal Price { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        [ForeignKey("Teacher")]
        public string TeacherId { get; set; } = null!;
        public Teacher? Teacher { get; set; }

        public ICollection<Lecture>? Lectures { get; set; } = new List<Lecture>();
        public ICollection<StudentCourse>? StudentCourses { get; set; }


    }
}