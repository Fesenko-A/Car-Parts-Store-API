using API.Interfaces;
using API.Utility;
using Common.Auth;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using BL.Services.Interfaces;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BrandController : ControllerBase, IProductDetails<BrandDto> {
        private readonly IBrandBL _bl;

        public BrandController(IBrandBL bl) {
            _bl = bl;
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
