using Microsoft.AspNetCore.Mvc;
using WebApi.Application.Models;
using WebApi.Application.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class CharacterController : ControllerBase
	{
		private readonly CharacterCollectionService _characterCollectionService;
		private readonly CharacterService _characterService;

		public CharacterController(CharacterCollectionService characterCollectionService, CharacterService characterService)
		{
			_characterCollectionService = characterCollectionService;
			_characterService = characterService;
		}

		[HttpGet("Create")]
		public async Task<IActionResult> CreateCharacter([FromQuery] string input)
		{
			try
			{
				var result = await _characterCollectionService.CreateCharacterAsync(input);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("Store")]
		public async Task<IActionResult> StoreCharacter([FromQuery] Character character)
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

		[HttpGet("GetCharactersByUserId")]
		public async Task<IActionResult> GetCharactersByUserId(int userId)
		{
			try
			{
				var result = await _characterCollectionService.GetCharactersByUserId(userId);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
