using Auth;
using BL;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase {
        private readonly PaymentMethodBL _bl;

        public PaymentMethodController() {
            _bl = new PaymentMethodBL();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll() {
            var methodsFromDb = await _bl.GetAll();
            return Ok(new ApiResponse(methodsFromDb));
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Create(PaymentMethodDto methodToCreate) {
            var result = await _bl.Create(methodToCreate);

            if (result.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }
    }
}
