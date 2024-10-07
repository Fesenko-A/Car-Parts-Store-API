using DAL.Repository.Models;

namespace DAL.Services.Interfaces {
    internal interface IShoppingCartDAL {
        Task<ShoppingCart?> Get(string userId);

        Task Create(ShoppingCart shoppingCartToAdd);

        Task Remove(ShoppingCart shoppingCartToRemove);
    }
}
