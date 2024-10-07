using Common.Auth;

namespace DAL.Services.Interfaces {
    internal interface IAuthDAL {
        Task<AppUser?> GetUser(string userName);
    }
}
