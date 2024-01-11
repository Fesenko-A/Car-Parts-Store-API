using BL;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase {
        private readonly ShoppingCartBL _bl;
        public ShoppingCartController() {
            _bl = new ShoppingCartBL();
        }

        [HttpGet]
        public async Task<ActionResult> Get(string userId) {
            var shoppingCart = await _bl.Get(userId);

            if (shoppingCart == null) {
                return BadRequest();
            }

            return Ok(shoppingCart);
        }

        [HttpPost]
        public async Task<ActionResult> Upsert(string userId, int productId, int updateQuantityBy) {
            bool isSuccess = await _bl.Upsert(userId, productId, updateQuantityBy);

            if (!isSuccess) {
                return BadRequest();
            }

            return Ok(true);
        }
    }
}
