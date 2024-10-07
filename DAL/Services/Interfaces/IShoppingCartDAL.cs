using DAL.Repository.Models;

namespace DAL.Services.Interfaces {
    public interface IShoppingCartDAL {
        Task<ShoppingCart?> Get(string userId);

        Task Create(ShoppingCart shoppingCartToAdd);

        Task Remove(ShoppingCart shoppingCartToRemove);
    }
}
