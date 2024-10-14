using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using BL.Models;
using API.Utility;
using Common.Auth;
using BL.Services.Interfaces;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpecialTagController : ControllerBase, IProductDetails<SpecialTagDto> {
        private readonly ISpecialTagBL _bl;

        public SpecialTagController(ISpecialTagBL bl) {
            _bl = bl;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll() {
            var specialTags = await _bl.GetAll();
            return Ok(new ApiResponse(specialTags));
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Create([FromForm] SpecialTagDto specialTagDto) {
            var result = await _bl.Create(specialTagDto);

            if (result.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }
    }
}
