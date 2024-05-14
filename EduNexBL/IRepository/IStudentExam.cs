using EduNexBL.Base;
using EduNexBL.DTOs;
using EduNexDB.Entites;

namespace EduNexBL.IRepository
{
    public interface IStudentExam : IRepository<StudentExam>
    {
        Task<StudentExam> GetStudentExam(string studentId, int examId);
        Task<List<StudentScoreDTO>> GetStudentsOrderedByScore(int examId);
    }
}