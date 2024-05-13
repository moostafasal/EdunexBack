using EduNexBL.Base;
using EduNexBL.IRepository;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduNexBL.Repository
{
    public class WalletRepo : Repository<Wallet>, IWallet
    {
        private readonly EduNexContext _Context;

        public WalletRepo(EduNexContext Context) : base(Context) 
        {
            _Context = Context;
        }

        public async Task<Wallet?> GetById(string id)
        {
            return await _Context.Wallets.SingleOrDefaultAsync(w => w.OwnerId == id);
        }

        public async Task UpdateWallet(Wallet wallet)
        {
            _Context.Wallets.Update(wallet);
            await _Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Wallet?>> GetALLWalletsByOwnerType(OwnerType ownerType)
        {
            return await _Context.Wallets.Where(w => w.OwnerType == ownerType).ToListAsync();
        }

        public async Task<IEnumerable<Wallet?>> GetALLWallets()
        {
            return await _Context.Wallets.ToListAsync();
        }

        public async Task<Wallet?> GetByOwnerIdAndOwnerType(string id, OwnerType ownerType)
        {
            return await _Context.Wallets.SingleOrDefaultAsync(w => w.OwnerId == id && w.OwnerType == ownerType);
        }

        public Wallet GenerateWallet(string ownerId)
        {
            var student = _Context.Students.Find(ownerId);
            var wallet = new Wallet()
            {
                Balance = 0,
                OwnerId = ownerId,
                OwnerType = student != null ? OwnerType.Student : OwnerType.Teacher
            };
            _Context.Wallets.Add(wallet);
            _Context.SaveChanges();
            return wallet;
        }
    }
}
