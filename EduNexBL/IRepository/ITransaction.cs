using EduNexBL.Base;
using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.IRepository
{
    public interface ITransaction : IRepository<Transaction>
    {
        Task<Transaction?> GetById(int id);
        Task<IEnumerable<Transaction>> GetTransactionByStudentId(string StudId);
        Task ADD(Transaction transaction);
    }
}
