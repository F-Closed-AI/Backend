using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Services;
using WebApi.Application.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomCollectionService _roomCollectionService;

        public RoomController(RoomCollectionService roomCollectionService)
        {
            _roomCollectionService = roomCollectionService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCharacter([FromBody] Room room)
        {
            try
            {
                var result = await _roomCollectionService.CreateRoom(room);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
