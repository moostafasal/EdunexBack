using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduNexDB.Entites
{
    public class AttachmentFile : BaseEntity
    {
        [Required]
        public string AttachmentTitle { get; set; } = null!;
        public string AttachmentPath { get; set; } = null!;

        [ForeignKey("Lecture")]
        public int LectureId { get; set; }
        public Lecture? Lecture { get; set; }
    }
}