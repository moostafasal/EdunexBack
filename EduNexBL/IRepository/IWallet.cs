using EduNexBL.Base;
using EduNexDB.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.IRepository
{
    public interface IWallet : IRepository<Wallet>
    {
        Task<Wallet?> GetById(string id);
        Task<Wallet?> GetByIdAndOwnerType(string id, string ownerType);
        Task UpdateWallet(Wallet wallet);
    }
}
