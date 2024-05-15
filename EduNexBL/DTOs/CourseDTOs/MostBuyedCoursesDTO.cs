using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.DTOs.CourseDTOs
{
    public class MostBuyedCoursesDTO : CourseMainData
    {
        public int? EnrollmentCount { get; set; }

    }
}
