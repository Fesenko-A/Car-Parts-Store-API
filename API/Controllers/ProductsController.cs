﻿using API.Utility;
using Common.Auth;
using BL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Common.Filters;
using BL.Services.Interfaces;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase {
        private readonly IProductBL _bl;

        public ProductsController(IProductBL bl) {
            _bl = bl;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetAll([FromQuery] ProductFilters filters) {
            // Item1 - list of orders, Item2 - totalRecords (pagination)
            var result = await _bl.GetAllProducts(filters);

            if (result.Item1.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Item1.Message));
            }

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(new Pagination(filters.PageNumber, filters.PageSize, result.Item2)));

            return Ok(new ApiResponse(result.Item1.Value));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApiResponse>> Get(int id) {
            var result = await _bl.GetProduct(id);

            if (result.Value == null) {
                return NotFound(new ApiResponse(HttpStatusCode.NotFound, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Create([FromForm] ProductDto productDto) {
            var result = await _bl.CreateProduct(productDto);

            if (result.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Update(int id, [FromForm] ProductDto productUpdateBody) {
            var result = await _bl.UpdateProduct(id, productUpdateBody);

            if (result.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.ADMIN)]
        public async Task<ActionResult<ApiResponse>> Delete(int id) {
            var result = await _bl.DeleteProduct(id);

            if (result?.Value == null) {
                return BadRequest(new ApiResponse(HttpStatusCode.BadRequest, false, result?.Message));
            }

            return Ok(new ApiResponse(result.Value));
        }
    }
}
