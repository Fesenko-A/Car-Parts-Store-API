using DAL.Repository.Models;

namespace DAL.Services.Interfaces {
    internal interface ICartItemDAL {
        Task Create(CartItem cartItemToAdd);

        Task Remove(CartItem cartItemToRemove);

        Task Update(CartItem cartItemToUpdate);
    }
}
