using MongoDB.Bson;
using MongoDB.Driver;
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
    }
}
