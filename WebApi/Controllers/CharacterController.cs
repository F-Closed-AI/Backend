using BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CharacterController : ControllerBase
	{
		[HttpGet]
		public async Task<IActionResult> GetCharacter([FromQuery] string input)
		{
			try
			{
				CharacterCollection charCollection = new CharacterCollection();

				var result = await charCollection.CreateCharacterAsync(input);

				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
