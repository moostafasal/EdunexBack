using EduNexBL.Base;
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
    public class StudentExamRepo : Repository<StudentExam>, IStudentExam
    {
        private readonly EduNexContext _context; 
        public StudentExamRepo(EduNexContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<StudentExam> GetStudentExam(string studentId, int examId)
        {
            return await _context.StudentExam
                .FirstOrDefaultAsync(se => se.StudentId == studentId && se.ExamId == examId);
        }
    }
}
