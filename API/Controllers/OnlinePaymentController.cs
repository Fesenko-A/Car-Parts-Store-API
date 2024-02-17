using BL;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OnlinePaymentController : ControllerBase {
        private readonly OnlinePaymentBL _bl;

        public OnlinePaymentController() {
            _bl = new OnlinePaymentBL();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll(string? userId, string? status) {
            var payments = await _bl.GetAll(userId, status);
            return Ok(new ApiResponse(payments));
        }

        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<ApiResponse>> GetByOrderId(int orderId) {
            var payment = await _bl.GetByOrderId(orderId);

            if (payment.Value == null) {
                return NotFound(new ApiResponse(HttpStatusCode.NotFound, false, payment.Message));
            }

            return Ok(new ApiResponse(payment.Value));
        }

        [HttpPost("{orderId:int}")]
        public async Task<ActionResult<ApiResponse>> Create(int orderId) {
            var payment = await _bl.Create(orderId);

            if (payment?.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, payment?.Message));
            }

            return Ok(new ApiResponse(payment.Value));
        }

        [HttpPut("{orderId:int}")]
        public async Task<ActionResult<ApiResponse>> Cancel(int orderId) {
            var cancelledPayment = await _bl.Cancel(orderId);

            if (cancelledPayment.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, cancelledPayment.Message));
            }

            return Ok(new ApiResponse(cancelledPayment.Value));
        }
    }
}
