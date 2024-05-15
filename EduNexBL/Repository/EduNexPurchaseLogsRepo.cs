using CloudinaryDotNet;
using EduNexBL.Base;
using EduNexBL.IRepository;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.Repository
{
    public class EduNexPurchaseLogsRepo : Repository<EduNexPurchaseLogs>, IEduNexPurchaseLogs
    {
        private readonly EduNexContext _Context;

        public EduNexPurchaseLogsRepo(EduNexContext Context) : base(Context)
        {
            _Context = Context;
        }
        public async Task<List<EduNexPurchaseLogs>> GetAllLogs()
        {
            return await _Context.EduNexPurchaseLogs.ToListAsync();
        }

        public async Task<IEnumerable<EduNexPurchaseLogs>> GetAllLogsByCourseId(int courseId)
        {
            return await _Context.EduNexPurchaseLogs.Where(e => e.CourseId == courseId).ToListAsync();
        }

        public async Task<IEnumerable<EduNexPurchaseLogs>> GetAllLogsByReceiverId(string teacherId)
        {
            return await _Context.EduNexPurchaseLogs.Where(e => e.ReceiverId == teacherId).ToListAsync();
        }

        public async Task<IEnumerable<EduNexPurchaseLogs>> GetAllLogsBySenderId(string studentId)
        {
            return await _Context.EduNexPurchaseLogs.Where(e => e.SenderId == studentId).ToListAsync();
        }

        public async Task<EduNexPurchaseLogs> GetById(string id)
        {
            return await _Context.EduNexPurchaseLogs.FindAsync(id);
        }

        public decimal CalculateBalance()
        {
            return _Context.EduNexPurchaseLogs.Select(e => e.AmountReceived).ToArray().Sum();
        }
    }
}
