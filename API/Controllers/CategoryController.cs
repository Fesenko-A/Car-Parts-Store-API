using BL.Models;
using BL;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase, IProductDetails<CategoryDto> {
        private readonly CategoryBL _bl;

        public CategoryController() {
            _bl = new CategoryBL();
        }

        [HttpGet]
        public async Task<ActionResult> GetAll() {
            var categories = await _bl.GetAll();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryDto categoryDto) {
            if (ModelState.IsValid) {
                var category = await _bl.Create(categoryDto);

                if (category == null) {
                    return BadRequest();
                }

                return Ok(category);
            }
            else {
                return BadRequest();
            }
        }
    }
}
