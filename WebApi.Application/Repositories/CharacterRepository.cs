using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Application.Interfaces;
using WebApi.Application.Models;

namespace WebApi.Application.Repositories
{
	public class CharacterRepository
	{
		private IMongoCollection<Character> _character;

		public CharacterRepository(IDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);
			_character = database.GetCollection<Character>(settings.CollectionName);
		}

		public async Task<Character> StoreCharacter(Character character)
		{
			await _character.InsertOneAsync(character);
			return character;
		}

		public async Task<Character> EditCharacter(Character character)
		{
			var filter = Builders<Character>.Filter.Eq("Id", character.Id);

			var update = Builders<Character>.Update
				.Set(chart => chart.Name, character.Name)
				.Set(chart => chart.Age, character.Age)
				.Set(chart => chart.Backstory, character.Backstory);
				

			await _character.UpdateOneAsync(filter, update);

			return character;
		}

		public List<Character> GetAll()
		{
			return _character.Find(character => true).ToList();
		}

		public async Task<List<Character>> GetAllByUserId(int id)
		{
			var filter = Builders<Character>.Filter.Eq("UserId", id);

			var groupStage = new BsonDocument("$group", new BsonDocument
			{
				{ "_id", "$CharId" },
				{ "latestCharacter", new BsonDocument("$first", "$$ROOT") }
			});

			var replaceRootStage = new BsonDocument("$replaceRoot", new BsonDocument("newRoot", "$latestCharacter"));

			var pipeline = new[] {
				PipelineStageDefinitionBuilder.Match(filter),
				PipelineStageDefinitionBuilder.Sort(Builders<Character>.Sort.Descending("_id")),
				groupStage,
				replaceRootStage
			};

			var cursor = await _character.AggregateAsync<Character>(PipelineDefinition<Character, Character>.Create(pipeline));
			return await cursor.ToListAsync();
		}
	}
}
