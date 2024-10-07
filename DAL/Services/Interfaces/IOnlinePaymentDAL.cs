using DAL.Repository.Models;

namespace DAL.Services.Interfaces {
    public interface IOnlinePaymentDAL {
        Task<OnlinePayment?> Get(int id);

        Task<List<OnlinePayment>> GetAll(string? userId, string? status);

        Task<OnlinePayment?> GetByOrderId(int id);

        Task Create(OnlinePayment newPayment);

        Task Update(OnlinePayment payment);
    }
}
