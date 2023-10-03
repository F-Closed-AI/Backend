using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BusinessLogic
{
	public class CharacterCollection
	{
		public void Test()
		{

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

			using (HttpClient client = new HttpClient())
			{
				try
				{
					StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

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
		}

		private string GetAnswerFromJson(string json)
		{
			try
			{
				using (JsonDocument doc = JsonDocument.Parse(json))
				{
					JsonElement root = doc.RootElement;
					if (root.TryGetProperty("output", out JsonElement outputElement) &&
						outputElement.TryGetProperty("answer", out JsonElement answerElement))
					{
						string answer = answerElement.GetString();
						return GetAnswerFromJson(answer);
					}
					else
					{
						return "JSON structure is not as expected.";
					}
				}
			}
			catch (JsonException ex)
			{
				return $"JSON parsing error: {ex.Message}";
			}
		}
	}
}
