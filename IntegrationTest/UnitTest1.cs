using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using WebApi;
using WebApi.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using WebApi.Application.Models;

namespace WebApi.Tests
{
    [TestFixture]
    public class CharacterControllerIntegrationTests
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
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Test]
        public async Task CreateCharacter_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var input = "invalidInput";

            // Act
            var response = await _client.GetAsync($"/api/Character/Create?input={input}");

            // Assert
            Assert.AreEqual(400, (int)response.StatusCode); // Bad Request
        }

        [Test]
        public async Task StoreCharacter_ValidCharacter_ReturnsOk()
        {
            // Arrange
            var character = new Character { /* Set valid properties */ };
            var json = JsonConvert.SerializeObject(character);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Character/Store", content);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Test]
        public async Task StoreCharacter_InvalidCharacter_ReturnsBadRequest()
        {
            // Arrange
            var character = new Character { /* Set invalid properties */ };
            var json = JsonConvert.SerializeObject(character);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Character/Store", content);

            // Assert
            Assert.AreEqual(400, (int)response.StatusCode); // Bad Request
        }
    }
}
