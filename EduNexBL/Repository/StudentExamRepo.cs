using EduNexBL.Base;
using EduNexBL.DTOs;
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

        private async Task<string> GetStudentNameById(string studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            return $"{student.FirstName} {student.LastName}";
        }

        public async Task<StudentExam> GetStudentExam(string studentId, int examId)
        {
            return await _context.StudentExam
                .FirstOrDefaultAsync(se => se.StudentId == studentId && se.ExamId == examId);
        }

        public async Task<List<StudentScoreDTO>> GetStudentsOrderedByScore(int examId)
        {
            var studentScores = await _context.StudentExam
                .Where(se => se.ExamId == examId && se.Score.HasValue)
                .OrderByDescending(se => se.Score)
                .Select(se => new
                {
                    se.StudentId,
                    se.Score
                })
                .ToListAsync();

            var result = new List<StudentScoreDTO>();

            foreach (var studentScore in studentScores)
            {
                var studentName = await GetStudentNameById(studentScore.StudentId);
                result.Add(new StudentScoreDTO
                {
                    StudentId = studentScore.StudentId,
                    StudentName = studentName,
                    Score = studentScore.Score.Value
                });
            }

            return result;
        }
    }
}
