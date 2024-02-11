using BL;
using BL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OnlinePaymentController : ControllerBase {
        private readonly OnlinePaymentBL _bl;
        private readonly ApiResponse _response;

        public OnlinePaymentController() {
            _bl = new OnlinePaymentBL();
            _response = new ApiResponse();
        }

        [HttpPost("{orderId:int}")]
        public async Task<ActionResult<ApiResponse>> Create(int orderId) {
            var payment = await _bl.Create(orderId);

            if (payment == null) {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            _response.Result = payment;
            return Ok(payment);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] OnlinePaymentDto paymentDto) {
            bool isSuccess = await _bl.Update(id, paymentDto);

            if (!isSuccess) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            return Ok(_response);
        }
    }
}
