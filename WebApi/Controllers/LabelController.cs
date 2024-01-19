using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Models;
using WebApi.Application.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly LabelService _labelService;

        public LabelController(LabelService labelService)
        {
            _labelService = labelService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLabel(string id)
        {
            try
            {
                var result = await _labelService.GetLabel(id);

                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetLabelsByUserId(int userId)
        {
            var result = await _labelService.GetAllByUserId(userId);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateLabel([FromBody] Label label)
        {
            try
            {
                var result = await _labelService.CreateLabel(label);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateLabel([FromBody] Label label)
        {
            try
            {
                var result = await _labelService.UpdateLabel(label.Id!, label);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteLabel([FromBody] string id)
        {
            try
            {
                var result = await _labelService.DeleteLabel(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
