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
        Task UpdateWallet(Wallet wallet);
        Task<IEnumerable<Wallet?>> GetALLWalletsByOwnerType(OwnerType ownerType);
        Task<IEnumerable<Wallet?>> GetALLWallets();
        Task<Wallet?> GetByOwnerIdAndOwnerType(string id, OwnerType ownerType);
        public Wallet GenerateWallet(string ownerId);
    }
}
