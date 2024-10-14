using API.Utility;
using BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase {
        private readonly IShoppingCartBL _bl;

        public ShoppingCartController(IShoppingCartBL bl) {
            _bl = bl;
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

            if (result.Value == false) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result?.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }
    }
}
