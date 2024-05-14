using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.CourseDTOs
{
    public class MostBuyedCoursesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public int? EnrollmentCount { get; set; }
        public Teacher? Teacher { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
