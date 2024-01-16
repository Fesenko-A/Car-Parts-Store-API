using BL;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase {
        private readonly PaymentBL _bl;
        private readonly ApiResponse _response;

        public PaymentController() {
            _bl = new PaymentBL();
            _response = new ApiResponse();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> MakePayment(string userId) {
            var shoppingCart = await _bl.MakePayment(userId);

            if (shoppingCart == null) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.Result = shoppingCart;
            return Ok(_response);
        }
    }
}
