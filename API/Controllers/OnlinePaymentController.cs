using BL;
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

        [HttpGet]
        public ActionResult<ApiResponse> GetAll(string? userId, string? status) {
            var payments = _bl.GetAll(userId, status);
            _response.Result = payments;

            return Ok(_response);
        }

        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<ApiResponse>> GetByOrderId(int orderId) {
            if (orderId == 0) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var payment = await _bl.GetByOrderId(orderId);

            if (payment == null) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.Result = payment;
            return Ok(_response);
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

        [HttpPut("{orderId:int}")]
        public async Task<ActionResult<ApiResponse>> Cancel(int orderId) {
            var cancelledPayment = await _bl.Cancel(orderId);

            if (cancelledPayment == null) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.Result = cancelledPayment;
            return Ok(_response);
        }
    }
}
