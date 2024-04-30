using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EduNexBL.DTOs.CourseDTOs
{
    public class VideoDTO
    {
        public int id { set; get; }
        public string VideoTitle { get; set; } = null!;

        public string VideoPath { get; set; } = null!;

    }
}