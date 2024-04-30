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
    public class LectureDto
    {
        public int? Id { set; get; } 

        public string LectureTitle { get; set; } = null!;

        public decimal Price { get; set; }

        public int CourseId { get; set; }

        public List<VideoDTO>? Videos { get; set; } = new List<VideoDTO>();
        public List<AttachmentDto>? Attachments { set; get; } = new List<AttachmentDto>();
        public int? PreExam { set; get; }
        public int? Assignment { set;  get; } 
    }
}
