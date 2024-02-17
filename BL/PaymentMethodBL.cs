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

        public async Task<ErrorOr<PaymentMethod>> Create(PaymentMethodDto paymentMethodToCreate) {
            PaymentMethod newPaymentMethod = new PaymentMethod {
                Description = paymentMethodToCreate.Description,
            };

            try {
                await _dal.Create(newPaymentMethod);
            }
            catch (DbUpdateException) {
                return new ErrorOr<PaymentMethod>("Error while creating Payment Method");
            }

            PaymentMethod paymentMethodFromDb = await _dal.Get(newPaymentMethod.PaymentMethodId);

            if (paymentMethodFromDb == null) {
                return new ErrorOr<PaymentMethod>("Error while getting Payment Method");
            }

            return new ErrorOr<PaymentMethod>(paymentMethodFromDb);
        }
    }
}
