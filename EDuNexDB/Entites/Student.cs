using EduNexDB.Entites;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduNexDB.Entites
{
    public class Student:ApplicationUser
    {
        public string ParentPhoneNumber { get; set; }
        [Required]
        public string Religion { get; set; }

        [ForeignKey("Level")]
        public int? LevelId { get; set; }
        public virtual Level? Level { get; set; }

        public ICollection<StudentExam>? StudentExams { get; set; }
    }
}
