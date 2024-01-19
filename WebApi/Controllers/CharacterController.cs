using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Models;
using WebApi.Application.Services;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CharacterController : ControllerBase
	{
		private readonly CharacterService _characterService;

		public CharacterController(CharacterService characterService)
		{
			_characterService = characterService;
		}

		[HttpPost("Create")]
		public async Task<IActionResult> CreateCharacter([FromBody] string input)
		{
			try
			{
				var result = await _characterService.CreateCharacterAsync(input);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("Store")]
		public async Task<IActionResult> StoreCharacter([FromBody] Character character)
		{
			try
			{
				var result = await _characterService.StoreCharacter(character);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("Update")]
		public async Task<IActionResult> UpdateCharacter([FromBody] Character character)
		{
			try
			{
				if (character.CharId == null)
				{
					return BadRequest("CharId is missing!");
				} else
				{
					var result = await _characterService.StoreCharacter(character);

					return Ok(result);
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("User/{userId}")]
		public async Task<IActionResult> GetCharactersByUserId(int userId)
		{
			try
			{
				var result = await _characterService.GetCharactersByUserId(userId);

				if (result.Any())
				{
					return Ok(result);
				} else
				{
					return NotFound();
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCharacter(string id)
		{
			try
			{
				var result = await _characterService.GetCharacter(id);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("Characters/{charId}")]
		public async Task<IActionResult> GetCharacters(string charId)
		{
			try
			{
				var result = await _characterService.GetCharacters(charId);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteCharacter([FromBody] string id)
        {
            try
            {
                var result = await _characterService.DeleteCharacter(id);

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
    }
}
