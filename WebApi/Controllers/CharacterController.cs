using BusinessLogic;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

				// Wacht op de resultaten van de asynchrone methode.
				var result = await charCollection.CreateCharacterAsync(input);

				// Retourneer de respons nadat de gegevens beschikbaar zijn.
				return Ok(result);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
