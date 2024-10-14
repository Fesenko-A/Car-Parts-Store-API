using Common.Auth;
using BL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAL.Services.Interfaces;
using BL.Services.Interfaces;

namespace BL.Services.Concrete {
    public class AuthBL : IAuthBL {
        private readonly IAuthDAL _dal;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthBL(IAuthDAL dal, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {
            _dal = dal;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ErrorOr<bool>> Register(RegisterRequestDto registerRequest) {
            AppUser? userFromDb = await _dal.GetUser(registerRequest.UserName);

            if (userFromDb != null) {
                return new ErrorOr<bool>("User with this username already exists");
            }

            AppUser newUser = new AppUser {
                UserName = registerRequest.UserName,
                Email = registerRequest.UserName,
                NormalizedEmail = registerRequest.UserName.ToUpper(),
                Name = registerRequest.Name
            };

            var result = await _userManager.CreateAsync(newUser, registerRequest.Password);
            if (result.Succeeded) {
                if (!_roleManager.RoleExistsAsync(Roles.ADMIN).GetAwaiter().GetResult()) {
                    await _roleManager.CreateAsync(new IdentityRole(Roles.ADMIN));
                    await _roleManager.CreateAsync(new IdentityRole(Roles.CUSTOMER));
                }

                if (registerRequest.Role.ToUpper() == Roles.ADMIN) {
                    await _userManager.AddToRoleAsync(newUser, Roles.ADMIN);
                }
                else {
                    await _userManager.AddToRoleAsync(newUser, Roles.CUSTOMER);
                }
            }

            return new ErrorOr<bool>(true);
        }

        public async Task<ErrorOr<LoginResponseDto>> Login(LoginRequestDto loginRequest) { 
            AppUser? userFromDb = await _dal.GetUser(loginRequest.UserName);
            if (userFromDb == null) {
                return new ErrorOr<LoginResponseDto>("User not found");
            }

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, loginRequest.Password);
            if (!isValid) {
                return new ErrorOr<LoginResponseDto>("Incorrect password");
            }

            var roles = await _userManager.GetRolesAsync(userFromDb);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(AuthOptions.KEY);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("fullName", userFromDb.Name),
                    new Claim("id", userFromDb.Id.ToString()),
                    new Claim(ClaimTypes.Email, userFromDb.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            LoginResponseDto loginResponse = new LoginResponseDto {
                Email = userFromDb.Email,
                Token = tokenHandler.WriteToken(token)
            };

            if (loginResponse.Email == null || loginResponse.Token == null) {
                return new ErrorOr<LoginResponseDto>("Error while logging in");
            }

            return new ErrorOr<LoginResponseDto>(loginResponse);
        }
    }
}
