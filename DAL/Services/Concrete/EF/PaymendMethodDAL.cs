﻿using DAL.Repository;
using DAL.Repository.Models;
using DAL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services.Concrete.EF
{
    public class PaymendMethodDAL : IPaymentMethodDAL {
        private readonly ApplicationDbContext _context;
        public PaymendMethodDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<List<PaymentMethod>> GetAll() {
            var paymentMethods = await _context.PaymentMethods.ToListAsync();
            return paymentMethods;
        }

        public async Task Create(PaymentMethod paymentMethodToCreate) {
            _context.PaymentMethods.Add(paymentMethodToCreate);
            await _context.SaveChangesAsync();
        }

        public async Task<PaymentMethod> Get(int id) {
            var paymentMethodFromDb = await _context.PaymentMethods.FirstOrDefaultAsync(x => x.PaymentMethodId == id);
            return paymentMethodFromDb;
        }
    }
}
