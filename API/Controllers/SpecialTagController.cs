using BL;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using Auth;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using BL.Models;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpecialTagController : ControllerBase, IProductDetails<SpecialTagDto> {
        private readonly SpecialTagBL _bl;
        private readonly ApiResponse _response;

        public SpecialTagController() {
            _bl = new SpecialTagBL();
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll() {
            var specialTags = await _bl.GetAll();
            _response.Result = specialTags;
            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Create([FromForm] SpecialTagDto specialTagDto) {
            if (ModelState.IsValid) {
                var specialTag = await _bl.Create(specialTagDto);

                if (specialTag == null) {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                _response.Result = specialTag;
                return Ok(_response);
            }
            else {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }
    }
}
