using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class CharacterController : ControllerBase
	{
		private readonly CharacterCollection _characterCollection;
		private readonly CharacterBL _characterBL;

		public CharacterController(CharacterCollection characterCollection, CharacterBL characterBL)
		{
			_characterCollection = characterCollection;
			_characterBL = characterBL;
		}

		[HttpGet("Create")]
		public async Task<IActionResult> CreateCharacter([FromQuery] string input)
		{
			try
			{
				var result = await _characterCollection.CreateCharacterAsync(input);

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
				var result = await _characterBL.StoreCharacter(character);

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
				var result = await _characterCollection.GetCharactersByUserId(userId);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
