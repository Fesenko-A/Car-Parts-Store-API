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
        private readonly ApiResponse _response;

        public PaymentMethodController() {
            _bl = new PaymentMethodBL();
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll() {
            var methodsFromDb = await _bl.GetAll();

            if (methodsFromDb == null) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.Result = methodsFromDb;
            return Ok(methodsFromDb);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Create(PaymentMethodDto methodToCreate) {
            if (ModelState.IsValid) {
                var paymentMethodCreated = await _bl.Create(methodToCreate);

                if (paymentMethodCreated == null) {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                _response.Result = paymentMethodCreated;
                return Ok(paymentMethodCreated);
            }
            else {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }
    }
}
