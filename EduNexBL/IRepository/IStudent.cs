using EduNexBL.Base;
using EduNexBL.DTOs;
using EduNexBL.DTOs.ExamintionDtos;
using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.IRepository
{
    public interface IStudent : IRepository<ApplicationUser>
    {
        Task<Student?> GetById(string id);
        Task<string?> GetStudentIdByNationalId(string nationalId);
        Task<List<StudentExamDTO>> GetExamsSubmissions(string studentId);
    }
}
