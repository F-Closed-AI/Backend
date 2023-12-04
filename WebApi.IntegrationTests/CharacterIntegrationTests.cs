using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using WebApi.Application.Models;

namespace WebApi.IntegrationTests
{
	[TestFixture]
	public class CharacterIntegrationTests
	{
		private WebApplicationFactory<WebApi.Program> _factory;
		public HttpClient _client { get; private set; }

		[SetUp]
		public void SetUp()
		{
			_factory = new WebApplicationFactory<WebApi.Program>();

			_client = _factory.CreateClient();
		}

		[TearDown]
		public void TearDown()
		{
			if (_client != null)
			{
				_client.Dispose();
			}

			if (_factory != null)
			{
				_factory.Dispose();
			}
		}


		[Test]
		public async Task CreateCharacter_ValidInput_ReturnsOk()
		{
			// Arrange
			var input = "validInput";

			// Act
			var response = await _client.GetAsync($"/api/Character/Create?input={input}");

			// Assert
			Assert.NotNull(response);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public async Task CreateCharacter_InvalidInput_ReturnsBadRequest()
		{
			// Act
			var response = await _client.GetAsync($"/api/Character/Create");

			// Assert
			Assert.NotNull(response);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}

		[Test]
		public async Task StoreCharacter_ValidCharacter_ReturnsOk()
		{
			// Arrange
			var character = new Character { UserId = 5, Name = "Test", Age = 5, Backstory = "Achtergrond verhaaltje" };
			var json = JsonConvert.SerializeObject(character);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			// Act
			var response = await _client.PostAsync("/api/Character/Store", content);

			// Assert
			Assert.NotNull(response);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public async Task StoreCharacter_InvalidCharacter_ReturnsBadRequest()
		{
			// Arrange
			var character = new Character();
			var json = JsonConvert.SerializeObject(character);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			// Act
			var response = await _client.PostAsync("/api/Character/Store", content);

			// Assert
			Assert.NotNull(response);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}
	}
}