using System;
using System.Linq;
using EduNexBL.ENums;
using EduNexDB.Context;
using EduNexDB.Entites;
using Microsoft.EntityFrameworkCore;

namespace EduNexBL.Services
{
    public class CouponService
    {
        private readonly EduNexContext _context;

        public CouponService(EduNexContext context)
        {
            _context = context;
        }

        // Method to generate a coupon
        public Coupon GenerateCoupon(decimal value, int numberOfUses, int daysValid, CouponType couponType)
        {
            var coupon = new Coupon
            {
                CouponCode = Guid.NewGuid().ToString().Substring(0, 8), // Generate a unique coupon code
                Value = value,
                NumberOfUses = numberOfUses,
                ExpirationDate = DateTime.Now.AddDays(daysValid), //Add days to currents date represents expiration date
                CouponType = couponType // 0 represents Charge_Coupon & 1 represents Discount_Coupon
            };
            _context.Coupon.Add(coupon);
            _context.SaveChanges();
            return coupon;
        }

        // Method to consume a coupon
        public bool ConsumeCoupon(string couponCode, string ownerId, string ownerType)
        {
            var coupon = _context.Coupon.FirstOrDefault(c => c.CouponCode == couponCode && c.NumberOfUses > 0
                        && c.ExpirationDate > DateTime.Now && c.CouponType == CouponType.Charge_Coupon);

            if (coupon == null)
            {
                return false; // Coupon not found or all uses have been exhausted or coupon has expired
            }

            // Get the wallet for the user or create one if it doesn't exist
            var wallet = _context.Wallets.FirstOrDefault(w => w.OwnerId == ownerId);
            if (wallet == null)
            {
                wallet = new Wallet
                {
                    OwnerId = ownerId,
                    OwnerType = ownerType,
                    Balance = coupon.Value
                };
                _context.Wallets.Add(wallet);
                _context.SaveChanges();
            }
            else
            {
                wallet.Balance += coupon.Value;
            }

            // Create a transaction for coupon usage
            var transaction = new Transaction
            {
                TransactionType = IntegrationType.coupon.ToString(),
                Amount = coupon.Value,
                TransactionDate = DateTime.Now.ToString(),
                WalletId = wallet.WalletId // Set the wallet ID for the transaction
            };

            _context.Transactions.Add(transaction);

            coupon.NumberOfUses--;

            _context.SaveChanges();

            return true;
        }

        //Get time left for the coupon before expiration
        public async Task<TimeSpan?> GetTimeLeftBeforeExpiration(string couponCode)
        {
            var coupon = await _context.Coupon.SingleOrDefaultAsync(c => c.CouponCode == couponCode);
            if (coupon != null && coupon.ExpirationDate > DateTime.Now && coupon.NumberOfUses > 0)
            {
                return coupon.ExpirationDate - DateTime.Now;
            }
            return TimeSpan.Zero;
        }
        public async Task<int> GetUsageNumberLeft(string couponCode)
        {
            var coupon = await _context.Coupon.SingleOrDefaultAsync(c => c.CouponCode == couponCode);
            if (coupon != null && coupon.ExpirationDate > DateTime.Now && coupon.NumberOfUses > 0)
            {
                return coupon.NumberOfUses;
            }
            return 0;
        }
    }
}
