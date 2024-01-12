using BL.Models;
using BL;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using Auth;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase, IProductDetails<CategoryDto> {
        private readonly CategoryBL _bl;
        private readonly ApiResponse _response;

        public CategoryController() {
            _bl = new CategoryBL();
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll() {
            var categories = await _bl.GetAll();
            _response.Result = categories;
            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Create(CategoryDto categoryDto) {
            if (ModelState.IsValid) {
                var category = await _bl.Create(categoryDto);

                if (category == null) {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                _response.Result = category;
                return Ok(_response);
            }
            else {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
        }
    }
}
