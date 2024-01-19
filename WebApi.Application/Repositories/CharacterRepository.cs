using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
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

		public async Task<List<Character>> GetAllByUserId(int userId)
		{
            var filter = Builders<Character>.Filter.And(
                Builders<Character>.Filter.Eq("UserId", userId),
                Builders<Character>.Filter.Eq("IsDeleted", false)
            );
            var sortDefinition = Builders<Character>.Sort.Descending("_id");

            var latestCharacters = (await _character.Find(filter)
                .Sort(sortDefinition)
                .ToListAsync())
                .GroupBy(character => character.CharId)
                .Select(group => group.First())
                .ToDictionary(character => character.CharId!);

            return latestCharacters.Values.ToList();
        }

		public async Task<Character> GetCharacter(string id)
		{
            var filter = Builders<Character>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _character.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Character>> GetCharacters(string charId)
        {
            var filter = Builders<Character>.Filter.Eq("CharId", charId);
            var sort = Builders<Character>.Sort.Descending("DateTime");
            return await _character.Find(filter)
                .Sort(sort)
                .ToListAsync();
        }

        public async Task<bool> DeleteCharacter(string id)
        {
            var character = await _character.Find(Builders<Character>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
            if (character == null) return false;

            var filter = Builders<Character>.Filter.Eq("CharId", character.CharId);
            var update = Builders<Character>.Update.Set("IsDeleted", true);

            var result = await _character.UpdateManyAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
