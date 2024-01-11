using DAL.Repository;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class ShoppingCartDAL {
        private readonly ApplicationDbContext _context;

        public ShoppingCartDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<ShoppingCart?> Get(string userId) {
            ShoppingCart? shoppingCart = await _context.ShoppingCarts.
                Include(s => s.CartItems).
                    ThenInclude(p => p.Product).
                    ThenInclude(p => p.SpecialTag).
                Include(s => s.CartItems).
                    ThenInclude(p => p.Product).
                    ThenInclude(p => p.Brand).
                Include(s => s.CartItems).
                    ThenInclude(p => p.Product).
                    ThenInclude(p => p.Category).
                FirstOrDefaultAsync(u => u.UserId == userId);

            return shoppingCart;
        } 

        public async Task Create(ShoppingCart shoppingCartToAdd) {
            _context.ShoppingCarts.Add(shoppingCartToAdd);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(ShoppingCart shoppingCartToRemove) {
            _context.ShoppingCarts.Remove(shoppingCartToRemove);
            await _context.SaveChangesAsync();
        }
    }
}
