using BL;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using BL.Models;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase {
        private readonly OrderBL _bl;
        private readonly ApiResponse _response;
        
        public OrderController() {
            _bl = new OrderBL();
            _response = new ApiResponse();
        }

        [HttpGet]
        public ActionResult<ApiResponse> GetAll(string? userId, string? searchString, string? status) {
            var ordersFromDb = _bl.GetAll(userId, searchString, status);

            if (ordersFromDb == null) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.Result = ordersFromDb;
            return Ok(_response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> Get(int id) {
            var ordersFromDb = await _bl.Get(id);

            if (ordersFromDb == null) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.Result = ordersFromDb;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create(OrderCreateDto orderToCreate) {
            if (ModelState.IsValid) {
                var orderCreated = await _bl.Create(orderToCreate);

                if (orderCreated == null) {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                _response.Result = orderCreated;
                return Ok(_response);
            }
            else {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> Update(int id, OrderUpdateDto orderToUpdate) {
            bool isSuccess = await _bl.Update(id, orderToUpdate);

            if (!isSuccess) {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            return Ok(_response);
        }
    }
}
