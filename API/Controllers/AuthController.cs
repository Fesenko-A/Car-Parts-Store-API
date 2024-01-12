using Auth;
using BL;
using BL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly AuthBL _bl;
        private readonly ApiResponse _response;

        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {
            _bl = new AuthBL(userManager, roleManager);
            _response = new ApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterRequestDto registerRequest) {
            bool isSuccess = await _bl.Register(registerRequest);
            if (!isSuccess) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRequestDto loginRequest) {
            LoginResponseDto? loginResponse = await _bl.Login(loginRequest);
            if (loginResponse == null) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.Result = loginResponse;
            return Ok(_response);
        }
    }
}
