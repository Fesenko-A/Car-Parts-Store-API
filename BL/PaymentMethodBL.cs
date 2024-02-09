using BL.Models;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BL {
    public class PaymentMethodBL {
        private readonly DAL.PaymendMethodDAL _dal;
        public PaymentMethodBL() {
            _dal = new DAL.PaymendMethodDAL();
        }

        public async Task<List<PaymentMethod>> GetAll() {
            var paymentMethods = await _dal.GetAll();
            return paymentMethods;
        }

        public async Task<PaymentMethod?> Create(PaymentMethodDto paymentMethodToCreate) {
            PaymentMethod newPaymentMethod = new PaymentMethod {
                Description = paymentMethodToCreate.Description,
            };

            try {
                await _dal.Create(newPaymentMethod);
            }
            catch (DbUpdateException) {
                return null;
            }

            PaymentMethod paymentMethodFromDb = await _dal.Get(newPaymentMethod.PaymentMethodId);
            return paymentMethodFromDb;
        }
    }
}
