using BL.Models;

namespace BL.Services.Interfaces {
    public interface IAuthBL {
        Task<ErrorOr<bool>> Register(RegisterRequestDto registerRequest);

        Task<ErrorOr<LoginResponseDto>> Login(LoginRequestDto loginRequest);
    }
}
