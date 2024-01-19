using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Services;
using WebApi.Application.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;
        private readonly CharacterService _characterService;

        public RoomController(RoomService roomService, CharacterService characterService)
        {
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

        [HttpGet("Rooms/{roomId}")]
        public async Task<IActionResult> GetRooms(string roomId)
        {
            try
            {
                var result = await _roomService.GetRooms(roomId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetRoomsByUserId(int userId)
        {
            var result = await _roomService.GetRoomsByUserId(userId);
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRoom([FromBody] Room room)
        {
            try
            {
                var result = await _roomService.CreateRoom(room);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateRoom([FromBody] Room room)
        {
            try
            {
                if (room.RoomId == null)
                {
                    return BadRequest("RoomId is missing!");
                }
                else
                {
                    var result = await _roomService.CreateRoom(room);

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteRoom([FromBody] string id)
        {
            try
            {
                var result = await _roomService.DeleteRoom(id);

                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Characters/{roomId}")]
        public async Task<IActionResult> GetCharacters(string roomId)
        {
            var result = await _roomService.GetCharacters(roomId);
            return Ok(result);
        }

        [HttpGet("Labels/{roomId}")]
        public async Task<IActionResult> GetLabels(string roomId)
        {
            var result = await _roomService.GetLabels(roomId);
            return Ok(result);
        }

        [HttpPost("CreateConversation/{roomId}")]
        public async Task<IActionResult> CreateConversation(string roomId, [FromBody] CreateConversation conversation)
        {
            try
            {
                if (roomId == null)
                {
                    return BadRequest("RoomId is missing!");
                }

                var char1 = await _characterService.GetCharacter(conversation.CharIds![0]);
                var char2 = await _characterService.GetCharacter(conversation.CharIds![1]);

                var result = await _roomService.CreateConversation(char1, char2, conversation.Subject!);

                return Ok(result);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
	}
}
