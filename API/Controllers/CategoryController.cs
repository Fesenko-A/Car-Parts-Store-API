using BL.Models;
using BL;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase {
        private readonly CategoryBL _bl;

        public CategoryController() {
            _bl = new CategoryBL();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCategories() {
            var categories = await _bl.GetAll();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(CategoryDto categoryDto) {
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
