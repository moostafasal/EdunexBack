using Amazon.Util.Internal;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EduNexBL.DTOs.CourseDTOs
{
    public class VideoAddDto
    {
        [Required]
        public string VideoTitle { get; set; }
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public int LectureId { get; set; }

    }

}