using DAL.Repository.Models;

namespace BL.Services.Interfaces {
    public interface IShoppingCartBL {
        Task<ErrorOr<ShoppingCart>> Get(string userId);

        Task<ErrorOr<bool>> Upsert(string userId, int productId, int updateQuantityBy);
    }
}
