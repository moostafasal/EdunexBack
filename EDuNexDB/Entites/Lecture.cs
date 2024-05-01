using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace EduNexDB.Entites
{
    public class Lecture : BaseEntity
    {
        [Required]
        public string LectureTitle { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public ICollection<Video> Videos { get; set; } = new List<Video>(); // Not nullable
        public ICollection<Exam> Exams { get; set; } = new List<Exam>(); // Not nullable
        public ICollection<AttachmentFile> Attachments { get; set; } = new List<AttachmentFile>(); // Not nullable
    }
}
