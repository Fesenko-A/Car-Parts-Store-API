using BL.Models;
using BL.Services.Interfaces;
using DAL.Repository.Models;
using DAL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BL.Services.Concrete {
    public class PaymentMethodBL : IPaymentMethodBL {
        private readonly IPaymentMethodDAL _dal;

        public PaymentMethodBL(IPaymentMethodDAL dal) {
            _dal = dal;
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
