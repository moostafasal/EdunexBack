using EduNexBL.Base;
using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.IRepository
{
    public interface IEduNexPurchaseLogs : IRepository<EduNexPurchaseLogs>
    {
        Task<EduNexPurchaseLogs?> GetById(string id);
        Task<IEnumerable<EduNexPurchaseLogs?>> GetAllLogsByReceiverId(string teacherId);
        Task<IEnumerable<EduNexPurchaseLogs?>> GetAllLogsBySenderId(string studentId);
        Task<IEnumerable<EduNexPurchaseLogs?>> GetAllLogsByCourseId(int courseId);
        Task<IEnumerable<EduNexPurchaseLogs?>> GetAllLogs();
        decimal CalculateBalance(); 
    }
}
