using BL;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase {
        private readonly ShoppingCartBL _bl;

        public ShoppingCartController() {
            _bl = new ShoppingCartBL();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> Get(string userId) {
            var result = await _bl.Get(userId);

            if (result.Value == null) {
                return NotFound(new ApiResponse(HttpStatusCode.NotFound, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Upsert(string userId, int productId, int updateQuantityBy) {
            var result = await _bl.Upsert(userId, productId, updateQuantityBy);

            if (result?.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result?.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }
    }
}
