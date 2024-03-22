using API.Utility;
using Common.Auth;
using BL;
using BL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly AuthBL _bl;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {
            _bl = new AuthBL(userManager, roleManager);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Register(RegisterRequestDto registerRequest) {
            var result = await _bl.Register(registerRequest);

            if (result?.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result?.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Login(LoginRequestDto loginRequest) {
            var result = await _bl.Login(loginRequest);

            if (result.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }
    }
}
