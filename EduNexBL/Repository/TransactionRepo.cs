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
    public class TransactionRepo : Repository<Transaction>, ITransaction
    {
        private readonly EduNexContext _Context;

        public TransactionRepo(EduNexContext Context) : base(Context)
        {
            _Context = Context;
        }

        public async Task ADD(Transaction transaction)
        {
            _Context.Transactions.Add(transaction);
            await _Context.SaveChangesAsync();
        }

        public async Task<Transaction?> GetById(int id)
        {
            return await _Context.Transactions.SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionByStudentId(string StudId)
        {
            Wallet targetWallet = await _Context.Wallets.SingleOrDefaultAsync(w => w.OwnerId == StudId);
            if (targetWallet != null)
            {
                return await _Context.Transactions.Where(t => t.WalletId == targetWallet.WalletId).ToListAsync();
            }
            return Enumerable.Empty<Transaction>();
        }
    }
}
