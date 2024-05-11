using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EduNexBL.DTOs.CourseDTOs
{
    public class AddUpdateCourseDTO
    {
        [Required]
        public string CourseName { get; set; } = null!;

        public IFormFile Thumbnail { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public String TeacherId { get; set; }

    }
}
