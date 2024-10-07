using Common.Auth;
using DAL.Repository;
using DAL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services.Concrete.EF
{
    public class AuthDAL : IAuthDAL {
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
