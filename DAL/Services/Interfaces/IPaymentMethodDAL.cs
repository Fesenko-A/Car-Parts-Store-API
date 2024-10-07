using DAL.Repository.Models;

namespace DAL.Services.Interfaces {
    public interface IPaymentMethodDAL {
        Task<List<PaymentMethod>> GetAll();

        Task Create(PaymentMethod paymentMethodToCreate);

        Task<PaymentMethod> Get(int id);
    }
}
