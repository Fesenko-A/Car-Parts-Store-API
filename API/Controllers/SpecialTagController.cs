using BL.Models;
using BL;
using Microsoft.AspNetCore.Mvc;
using DAL.Repository.Models;

namespace API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SpecialTagController : ControllerBase {
        private readonly SpecialTagBL _bl;

        public SpecialTagController() {
            _bl = new SpecialTagBL();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllSpecialTags() {
            var specialTags = await _bl.GetAllSpecialTags();
            return Ok(specialTags);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSpecialTag(SpecialTagDto specialTagDto) {
            if (ModelState.IsValid) {
                var specialTag = await _bl.CreateSpecialTag(specialTagDto);

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
