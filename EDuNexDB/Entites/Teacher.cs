using System.ComponentModel.DataAnnotations.Schema;

namespace EduNexDB.Entites
{
    public enum TeacherStatus
    {
        Pending,
        Approved,
        Rejected
    }
    public class Teacher:ApplicationUser
    {

        public string? ProfilePhoto { get; set; }

        public string? Description { get; set; }
        public string? FacebookAccount { get; set; }


        [Column("AboutMe")]
        public string? AboutMe { get; set; }
        public string? AccountNote { get; set; }

        public string? subject { get; set; }

        public TeacherStatus Status { get; set; } = TeacherStatus.Pending; // Set default status to Pending

    }
}
