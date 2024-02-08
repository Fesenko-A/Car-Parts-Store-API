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
        private readonly ApiResponse _response;

        public BrandController() {
            _bl = new BrandBL();
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll() {
            var brands = await _bl.GetAll();
            _response.Result = brands;
            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Create([FromForm] BrandDto brandDto) {
            if (ModelState.IsValid) {
                var brand = await _bl.Create(brandDto);

                if (brand == null) {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                _response.Result = brand;
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
