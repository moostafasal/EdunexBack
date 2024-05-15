using EduNexBL.Base;
using EduNexBL.DTOs.ExamintionDtos;
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
    public class StudentRepo : Repository<ApplicationUser>, IStudent
    {
        private readonly EduNexContext _Context;

        public StudentRepo(EduNexContext Context) : base(Context)
        {
            _Context = Context;
        }
        public async Task<Student?> GetById(string id)
        {
            return await _Context.Students.SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<string?> GetStudentIdByNationalId(string nationalId)
        {
            return await _Context.Students
                .Where(s => s.NationalId == nationalId)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();
        }
        public async Task<List<StudentExamDTO>> GetExamsSubmissions(string studentId)
        {
            return await _Context.StudentExam
                .Where(se => se.StudentId == studentId)
                .Select(se => new StudentExamDTO
                {
                    StudentId = se.StudentId,
                    ExamId = se.ExamId,
                    Score = se.Score,
                    StartTime = se.StartTime,
                    EndTime = se.EndTime
                }).ToListAsync();
        }
    }
}
