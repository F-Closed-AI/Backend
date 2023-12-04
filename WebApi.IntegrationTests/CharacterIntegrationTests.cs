using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
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
			var projectDir = Directory.GetCurrentDirectory();
			var configPath = Path.Combine(projectDir, "appsettings.Test.json");

			_factory = new WebApplicationFactory<WebApi.Program>()
				.WithWebHostBuilder(builder =>
				{
					builder.ConfigureAppConfiguration((context, config) =>
					{
						config.AddJsonFile(configPath);
					});
				});
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

		[Test]
		public async Task UpdateCharacter()
		{
			// arrange
			string id = ""; // is leeg
			int userid = 5;
			string charid = "656da8572622a5a4bc79e5a0";
			string Name = "woah een naam";
			int age = 99;
			string backstory = "update op backstory";
			//act
			var response = await _client.PostAsync($"/api/Character/Update?Id={id}&CharId={charid}&UserId={userid}&Name={Name}&Age={age}&Backstory={backstory}", null);
			//assert
			Assert.IsNotNull(response);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public async Task UpdateCharacter_NotExistantCharacter()
		{
			//arrange
			string id = ""; // is leeg
			int userid = 5;
			string charid = "dit kan echt totaal niet";
			string Name = "woah een naam";
			int age = 99;
			string backstory = "update op backstory";
			//act
			var response = await _client.PostAsync($"/api/Character/Update?Id={id}&CharId={charid}&UserId={userid}&Name={Name}&Age={age}&Backstory={backstory}", null);
			//assert
			Assert.NotNull(response);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}

		[Test]
		public async Task GetCharacterByUserId()
		{
			//arrange
			int userid = 5;
			//act
			var response = await _client.GetAsync($"/api/Character/GetCharactersByUserId?userId={userid}");
			//assert
			Assert.NotNull(response);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public async Task GetCharacterByUserId_WrongUserId()
		{
			//arrange
			int userid = 666666;
			//act
			var response = await _client.GetAsync($"/api/Character/GetCharactersByUserId?userId={userid}");
			//assert
			Assert.NotNull(response);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}

		[Test]
		public async Task GetCharacterByCharacterId()
		{
			//arange
			string characterid = "6565cedfe9137b8bc60df71f";
			//act
			var response = await _client.GetAsync($"/api/Character/GetCharacter?charId={characterid}");
			//assert
			Assert.NotNull(response);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}
		[Test]
		public async Task GetCharacterByCharacterId_WrongCharacterId()
		{
			//arange
			string characterid = "woah";
			//act
			var response = await _client.GetAsync($"/api/Character/GetCharacter?charId={characterid}");
			//assert
			Assert.NotNull(response);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}
	}
}