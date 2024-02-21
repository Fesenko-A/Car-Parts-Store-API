using BL;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Auth;
using Azure;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase {
        private readonly OrderBL _bl;
        
        public OrderController() {
            _bl = new OrderBL();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll(string? userId, string? searchString, string? status) {
            var ordersFromDb = await _bl.GetAll(userId, searchString, status);
            return Ok(new ApiResponse(ordersFromDb));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> Get(int id) {
            var result = await _bl.Get(id);

            if (result.Value == null) {
                return NotFound(new ApiResponse(HttpStatusCode.NotFound, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create(OrderCreateDto orderToCreate) {
            var result = await _bl.Create(orderToCreate);

            if (result.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }

        [Authorize(Roles = Roles.CUSTOMER)]
        [Authorize(Roles = Roles.ADMIN)]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] OrderUpdateDto orderToUpdate) {
            var result = await _bl.Update(id, orderToUpdate);

            if (result.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }
    }
}
