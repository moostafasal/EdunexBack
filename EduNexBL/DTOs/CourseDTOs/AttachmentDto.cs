using System.ComponentModel.DataAnnotations;

namespace EduNexBL.DTOs.CourseDTOs
{
    public class AttachmentDto
    {
        public int Id { get; set; }
        public string AttachmentTitle { get; set; } = null!;
        public string? AttachmentPath { get; set; } = null!;

    }

}