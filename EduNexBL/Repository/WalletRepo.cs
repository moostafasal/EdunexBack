using EduNexBL.Base;
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

    }
}
