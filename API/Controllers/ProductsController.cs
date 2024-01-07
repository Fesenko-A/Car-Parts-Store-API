using BL;
using BL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase {
        private readonly ProductBL _BL;

        public ProductsController() {
            _BL = new ProductBL();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProducts() {
            var products = await _BL.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetProduct(int id) {
            var product = await _BL.GetProduct(id);

            if (id == 0) {
                return BadRequest();
            }

            if (product == null) {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductDto productDto) {
            if (ModelState.IsValid) {
                var product = await _BL.CreateProduct(productDto);

                if (product == null) {
                    return BadRequest();
                }

                return Ok(product);
            }
            else {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(int id, ProductDto productUpdateBody) {
            if (id == 0 || !ModelState.IsValid) {
                return BadRequest();
            }

            var product = await _BL.UpdateProduct(id, productUpdateBody);
            if (product == null) {
                return BadRequest();
            }

            return Ok(product);
        }
    }
}
