using DAL.Repository;
using DAL.Repository.Models;
using DAL.Services.Interfaces;

namespace DAL.Services.Concrete.EF {
    public class CartItemDAL : ICartItemDAL {
        private readonly ApplicationDbContext _context;

        public CartItemDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task Create(CartItem cartItemToAdd) {
            _context.CartItems.Add(cartItemToAdd);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(CartItem cartItemToRemove) {
            _context.CartItems.Remove(cartItemToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task Update(CartItem cartItemToUpdate) {
            _context.CartItems.Update(cartItemToUpdate);
            await _context.SaveChangesAsync();
        }
    }
}
