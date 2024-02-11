using Auth;
using BL;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase {
        private readonly ProductBL _bl;
        private readonly ApiResponse _response;

        public ProductsController() {
            _bl = new ProductBL();
            _response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll() {
            var products = await _bl.GetAllProducts();
            _response.Result = products;
            return Ok(_response);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> Get(int id) {
            var product = await _bl.GetProduct(id);

            if (id == 0) {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            if (product == null) {
                _response.StatusCode = HttpStatusCode.NotFound;
                _response.IsSuccess = false;
                return NotFound(_response);
            }

            _response.Result = product;
            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Create([FromForm] ProductDto productDto) {
            if (ModelState.IsValid) {
                var product = await _bl.CreateProduct(productDto);

                if (product == null) {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }

                _response.Result = product;
                return Ok(_response);
            }
            else {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromForm] ProductDto productUpdateBody) {
            if (id == 0 || !ModelState.IsValid) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var product = await _bl.UpdateProduct(id, productUpdateBody);
            if (product == null) {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                return BadRequest(_response);
            }

            _response.Result = product;
            return Ok(_response);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Delete(int id) {
            if (id == 0) { 
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            bool success = await _bl.DeleteProduct(id);
            if (!success) {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
