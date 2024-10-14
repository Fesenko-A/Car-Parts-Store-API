using BL.Models;
using DAL.Repository.Models;

namespace BL.Services.Interfaces {
    public interface IPaymentMethodBL {
        Task<List<PaymentMethod>> GetAll();

        Task<ErrorOr<PaymentMethod>> Create(PaymentMethodDto paymentMethodToCreate);
    }
}
