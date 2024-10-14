using BL.Models;
using DAL.Repository.Models;

namespace BL.Services.Interfaces {
    public interface IOnlinePaymentBL {
        Task<List<OnlinePayment>> GetAll(string? userId, string? status);

        Task<ErrorOr<OnlinePayment?>> GetByOrderId(int orderId);

        Task<ErrorOr<OnlinePayment>> Create(int orderId, string paymentId);

        Task<ErrorOr<StripeIntent>> CreateIntent(int orderId);

        Task<ErrorOr<OnlinePayment>> Cancel(int orderId);

        Task<bool> Update(int id, OnlinePayment paymentToUpdate);
    }
}
