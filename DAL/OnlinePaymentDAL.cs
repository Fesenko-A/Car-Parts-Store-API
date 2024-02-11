using DAL.Repository;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class OnlinePaymentDAL {
        private readonly ApplicationDbContext _context;

        public OnlinePaymentDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<OnlinePayment?> Get(int id) {
            var paymentFromDb = await _context.OnlinePayments.Where(i => i.Id == id).FirstOrDefaultAsync();
            return paymentFromDb;
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
