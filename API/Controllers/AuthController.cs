using Auth;
using BL;
using BL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly AuthBL _bl;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {
            _bl = new AuthBL(userManager, roleManager);
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterRequestDto registerRequest) {
            bool isSuccess = await _bl.Register(registerRequest);
            if (!isSuccess) {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRequestDto loginRequest) {
            LoginResponseDto? loginResponse = await _bl.Login(loginRequest);
            if (loginResponse == null) {
                return BadRequest();
            }
            return Ok(loginResponse);
        }
    }
}
