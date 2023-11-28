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

		public async Task<List<Character>> GetCharacter(string charId)
		{
			var filter = Builders<Character>.Filter.Eq("CharId", charId);

			var sort = Builders<Character>.Sort.Descending("_id");

			var result = await _character.Find(filter).Sort(sort).ToListAsync();

			return result;
		}
	}
}
