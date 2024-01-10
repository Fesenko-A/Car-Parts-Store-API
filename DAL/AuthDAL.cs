using Auth;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class AuthDAL {
        private readonly ApplicationDbContext _context;

        public AuthDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<AppUser?> GetUser(string userName) {
            AppUser? userFromDb = await _context.AppUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
            return userFromDb;
        }
    }
}
