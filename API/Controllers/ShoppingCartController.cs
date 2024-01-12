using BL;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase {
        private readonly ShoppingCartBL _bl;
        private readonly ApiResponse _response;

        public ShoppingCartController() {
            _bl = new ShoppingCartBL();
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> Get(string userId) {
            var shoppingCart = await _bl.Get(userId);

            if (shoppingCart == null) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.Result = shoppingCart;
            return Ok(_response);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Upsert(string userId, int productId, int updateQuantityBy) {
            bool isSuccess = await _bl.Upsert(userId, productId, updateQuantityBy);

            if (!isSuccess) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            return Ok(_response);
        }
    }
}
