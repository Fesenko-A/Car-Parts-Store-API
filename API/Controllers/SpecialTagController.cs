using BL;
using Microsoft.AspNetCore.Mvc;
using DAL.Repository.Models;
using API.Interfaces;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpecialTagController : ControllerBase, IProductDetails<SpecialTagDto> {
        private readonly SpecialTagBL _bl;

        public SpecialTagController() {
            _bl = new SpecialTagBL();
        }

        [HttpGet]
        public async Task<ActionResult> GetAll() {
            var specialTags = await _bl.GetAll();
            return Ok(specialTags);
        }

        [HttpPost]
        public async Task<ActionResult> Create(SpecialTagDto specialTagDto) {
            if (ModelState.IsValid) {
                var specialTag = await _bl.Create(specialTagDto);

                if (specialTag == null) {
                    return BadRequest();
                }

                return Ok(specialTag);
            }
            else {
                return BadRequest();
            }
        }
    }
}
