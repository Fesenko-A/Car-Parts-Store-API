using BL.Models;
using BL;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using Auth;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using API.Utility;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase, IProductDetails<CategoryDto> {
        private readonly CategoryBL _bl;

        public CategoryController() {
            _bl = new CategoryBL();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll() {
            var categories = await _bl.GetAll();
            return Ok(new ApiResponse(categories));
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Create([FromForm] CategoryDto categoryDto) {
            var result = await _bl.Create(categoryDto);

            if (result.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }
    }
}
