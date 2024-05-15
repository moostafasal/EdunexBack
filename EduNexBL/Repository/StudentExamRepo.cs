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

        public async Task<List<StudentScoreDTO>> GetStudentTotalScores()
        {
            var studentScores = new List<StudentScoreDTO>();

            // Retrieve students with related exams
            var studentsWithExams = _context.Students
                .Include(s => s.StudentExams)
                .ToList();

            foreach (var student in studentsWithExams)
            {
                int totalScore = 0;

                // Calculate total score
                foreach (var studentExam in student.StudentExams)
                {
                    if (studentExam.Score.HasValue)
                    {
                        totalScore += studentExam.Score.Value;
                    }
                }

                // Create DTO object for student score
                var studentScoreDTO = new StudentScoreDTO
                {
                    StudentName = student.FirstName +" "+ student.LastName, 
                    Score = totalScore
                };

                studentScores.Add(studentScoreDTO);
            }

            // Sort the student scores in descending order based on total score
            studentScores = studentScores.OrderByDescending(s => s.Score).ToList();

            return studentScores;
        }
    }
}
