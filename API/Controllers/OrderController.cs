using BL;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using API.Utility;
using System.Text.Json;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase {
        private readonly OrderBL _bl;
        
        public OrderController() {
            _bl = new OrderBL();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll(string? userId, string? searchString, string? status, int pageNumber = 1, int pageSize = 5) {
            // Item1 - list of orders, Item2 - totalRecords (pagination)
            var result = await _bl.GetAll(userId, searchString, status, pageNumber, pageSize);

            if (result.Item1.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Item1.Message));
            }

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(new Pagination(pageNumber, pageSize, result.Item2)));

            return Ok(new ApiResponse(result.Item1.Value));
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> Get(int id) {
            var result = await _bl.Get(id);

            if (result.Value == null) {
                return NotFound(new ApiResponse(HttpStatusCode.NotFound, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Create(OrderCreateDto orderToCreate) {
            var result = await _bl.Create(orderToCreate);

            if (result.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }

        [Authorize]
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
