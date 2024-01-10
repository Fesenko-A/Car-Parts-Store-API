using Auth;
using BL;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase {
        private readonly ProductBL _bl;

        public ProductsController() {
            _bl = new ProductBL();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts() {
            var products = await _bl.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProduct(int id) {
            var product = await _bl.GetProduct(id);

            if (id == 0) {
                return BadRequest();
            }

            if (product == null) {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult> CreateProduct(ProductDto productDto) {
            if (ModelState.IsValid) {
                var product = await _bl.CreateProduct(productDto);

                if (product == null) {
                    return BadRequest();
                }

                return Ok(product);
            }
            else {
                return BadRequest();
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult> UpdateProduct(int id, ProductDto productUpdateBody) {
            if (id == 0 || !ModelState.IsValid) {
                return BadRequest();
            }

            var product = await _bl.UpdateProduct(id, productUpdateBody);
            if (product == null) {
                return BadRequest();
            }

            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult> DeleteProduct(int id) {
            if (id == 0) {
                return BadRequest();
            }

            bool success = await _bl.DeleteProduct(id);
            if (!success) {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
