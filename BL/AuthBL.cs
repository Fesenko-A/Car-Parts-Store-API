using Auth;
using BL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BL
{
    public class AuthBL {
        private readonly DAL.AuthDAL _dal;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthBL(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {
            _dal = new DAL.AuthDAL();
            _userManager = userManager; 
            _roleManager = roleManager;
        }

        public async Task<bool> Register(RegisterRequestDto registerRequest) {
            AppUser? userFromDb = await _dal.GetUser(registerRequest.UserName);

            if (userFromDb != null) {
                return false;
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

            return true;
        }
    
        public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequest) {
            AppUser? userFromDb = await _dal.GetUser(loginRequest.UserName);
            if (userFromDb == null) {
                return null;
            }

            bool isValid = await _userManager.CheckPasswordAsync(userFromDb, loginRequest.Password);
            if (!isValid) {
                return null;
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
                return null;
            }

            return loginResponse;
        }
    }
}
