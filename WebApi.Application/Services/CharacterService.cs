using System.Text.Json;
using System.Text;
using WebApi.Application.Models;
using WebApi.Application.Repositories;

namespace WebApi.Application.Services
{
	public class CharacterService
	{
		private readonly CharacterRepository _characterRepository;
		public CharacterService(CharacterRepository characterRepository)
		{
			_characterRepository = characterRepository;
		}

		public async Task<string> CreateCharacterAsync(string input)
		{
			string apiUrl = "https://api-d7b62b.stack.tryrelevance.com/latest/studios/cafafd66-ee8c-4764-ba15-65fcd86ec9b7/trigger_limited";

			string jsonBody = $@"{{
				""params"": {{
					""long_text"": ""{input}""
				}},
				""project"": ""6f2b3a705849-4ac5-b8df-d50bd22fde95""
			}}";

			using HttpClient client = new();

			try
			{
				StringContent content = new(jsonBody, Encoding.UTF8, "application/json");

				HttpResponseMessage response = await client.PostAsync(apiUrl, content);

				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();

					return responseBody;
				}
				else
				{
					return $"Fout: Statuscode {response.StatusCode}";
				}
			}
			catch (Exception ex)
			{
				return $"Fout: {ex.Message}";
			}
		}

		private string GetAnswerFromJson(string json)
		{
			try
			{
				using JsonDocument doc = JsonDocument.Parse(json);

				JsonElement root = doc.RootElement;
				if (root.TryGetProperty("output", out JsonElement outputElement) &&
					outputElement.TryGetProperty("answer", out JsonElement answerElement))
				{
					string? answer = answerElement.GetString();
					if (answer != null)
					{
						return GetAnswerFromJson(answer);
					}
					else
					{
						return "Answer is null";
					}
				}
				else
				{
					return "JSON structure is not as expected.";
				}
			}
			catch (JsonException ex)
			{
				return $"JSON parsing error: {ex.Message}";
			}
		}

		public async Task<List<Character>> GetCharactersByUserId(int id)
		{
			try
			{
				return await _characterRepository.GetAllByUserId(id);
			}
			catch (JsonException ex)
			{
				throw new Exception($"JSON parsing error: {ex.Message}");
			}
		}

		public async Task<bool> DeleteCharacter(string id)
		{
			return await _characterRepository.DeleteCharacter(id);
		}

		public async Task<Character> StoreCharacter(Character character)
		{
			return await _characterRepository.StoreCharacter(character);
		}

		public async Task<Character> GetCharacter(string id)
		{
			return await _characterRepository.GetCharacter(id);
		}

		public async Task<IEnumerable<Character>> GetCharacters(string charId)
		{
			return await _characterRepository.GetCharacters(charId);
		}
	}
}
