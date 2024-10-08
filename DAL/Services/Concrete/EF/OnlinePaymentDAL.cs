﻿using DAL.Repository;
using DAL.Repository.Models;
using DAL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services.Concrete.EF
{
    public class OnlinePaymentDAL : IOnlinePaymentDAL {
        private readonly ApplicationDbContext _context;

        public OnlinePaymentDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<OnlinePayment?> Get(int id) {
            var paymentFromDb = await _context.OnlinePayments.Where(i => i.Id == id).FirstOrDefaultAsync();
            return paymentFromDb;
        }

        public async Task<List<OnlinePayment>> GetAll(string? userId, string? status) {
            IQueryable<OnlinePayment> payments = _context.OnlinePayments.Include(o => o.Order);

            if (!string.IsNullOrEmpty(userId)) {
                payments = payments.Where(u => u.UserId == userId);
            }

            if (!string.IsNullOrEmpty(status)) {
                payments = payments.Where(s => s.PaymentStatus == status);
            }

            return await payments.ToListAsync();
        }

        public async Task<OnlinePayment?> GetByOrderId(int id) {
            var paymentFromDb = await _context.OnlinePayments.Where(i => i.OrderId == id).FirstOrDefaultAsync();
            return paymentFromDb;
        }

        public async Task Create(OnlinePayment newPayment) {
            _context.OnlinePayments.Add(newPayment);
            await _context.SaveChangesAsync();
        }

        public async Task Update(OnlinePayment payment) {
            _context.OnlinePayments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
