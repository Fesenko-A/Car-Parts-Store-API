using API.Utility;
using Common.Auth;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BL.Services.Interfaces;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase {
        private readonly IPaymentMethodBL _bl;

        public PaymentMethodController(IPaymentMethodBL bl) {
            _bl = bl;
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
