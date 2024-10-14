using API.Utility;
using Common.Auth;
using BL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BL.Services.Interfaces;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthBL _bl;

        public AuthController(IAuthBL bl, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {
            _bl = bl;
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
