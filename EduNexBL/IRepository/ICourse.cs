using EduNexBL.Base;
using EduNexBL.DTOs.CourseDTOs;
using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.IRepository
{
    public interface ICourse : IRepository<Course>
    {
        public Task<ICollection<CourseMainData>> GetAllCoursesMainData(); 
    }
}
