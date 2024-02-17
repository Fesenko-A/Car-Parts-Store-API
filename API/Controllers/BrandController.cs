using API.Interfaces;
using Auth;
using BL;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BrandController : ControllerBase, IProductDetails<BrandDto> {
        private readonly BrandBL _bl;

        public BrandController() {
            _bl = new BrandBL();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll() {
            var brands = await _bl.GetAll();
            return Ok(new ApiResponse(brands));
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Create([FromForm] BrandDto brandDto) {
            var result = await _bl.Create(brandDto);

            if (result.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }
    }
}
