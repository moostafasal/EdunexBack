using AutoMapper;
using EduNexBL.Base;
using EduNexBL.DTOs.CourseDTOs;
using EduNexBL.IRepository;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.Repository
{
    public class CourseRepo : Repository<Course>, ICourse
    {
        private readonly IMapper _mapper;
        private readonly EduNexContext _context; 
        public CourseRepo(EduNexContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
            _context = dbContext;
        }

        public async Task<ICollection<CourseMainData>> GetAllCoursesMainData()
        {
            var courses = await _context.Courses
                .Include(c => c.Teacher)
                .Include(c => c.Subject)
                .ThenInclude(s => s.Level)
                .ToListAsync();

            return _mapper.Map<List<CourseMainData>>(courses);
        }
    }
}
