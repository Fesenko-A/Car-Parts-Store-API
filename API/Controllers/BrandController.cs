using API.Interfaces;
using BL;
using BL.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BrandController : ControllerBase, IProductDetails<BrandDto> {
        private readonly BrandBL _bl;

        public BrandController() {
            _bl = new BrandBL();
        }

        [HttpGet]
        public async Task<ActionResult> GetAll() {
            var brands = await _bl.GetAll();
            return Ok(brands);
        }

        [HttpPost]
        public async Task<ActionResult> Create(BrandDto brandDto) {
            if (ModelState.IsValid) {
                var brand = await _bl.Create(brandDto);

                if (brand == null) {
                    return BadRequest();
                }

                return Ok(brand);
            }
            else {
                return BadRequest();
            }
        }
    }
}
