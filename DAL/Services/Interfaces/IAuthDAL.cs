using Common.Auth;

namespace DAL.Services.Interfaces {
    public interface IAuthDAL {
        Task<AppUser?> GetUser(string userName);
    }
}
