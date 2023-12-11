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
        private readonly RoomService _roomService;
        private readonly CharacterService _characterService;

        public RoomController(RoomCollectionService roomCollectionService, RoomService roomService, CharacterService characterService)
        {
            _roomCollectionService = roomCollectionService;
            _roomService = roomService;
            _characterService = characterService;
        }

		[HttpGet("{id}")]
		public async Task<IActionResult> GetRoom(string id)
		{
			try
			{
				var result = await _roomService.GetRoom(id);

                if (result == null)
                {
                    return NotFound();
                } else
                {
				    return Ok(result);
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("Create")]
        public async Task<IActionResult> CreateRoom([FromBody] Room room)
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

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            try
            {
                var result = await _roomCollectionService.DeleteRoom(id);

                if (result)
                {
                    return Ok(result);
                } else
                {
                    return BadRequest();
                }
            }
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("AddCharacter")]
		public async Task<IActionResult> AddCharacter(string roomId, string charId)
		{
			try
			{
				var character = await _characterService.GetCharacter(charId);

                if (character == null)
                {
                    return NotFound("Character not found");
                }

				var result = await _roomService.AddCharacter(roomId, charId);

                if (result == null)
                {
                    return NotFound("Room not found");
                }

                return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
