using EduNexBL.Base;
using EduNexDB.Entites;

namespace EduNexBL.IRepository
{
    public interface IStudentExam : IRepository<StudentExam>
    {
        Task<StudentExam> GetStudentExam(string studentId, int examId); 
    }
}