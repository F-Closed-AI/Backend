using Interfaces;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessMD
{
	public class CharacterMD
	{
		private IMongoCollection<Character> _character;

		public CharacterMD(IDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);
			_character = database.GetCollection<Character>(settings.CollectionName);
		}

		public async Task<Character> Create(Character character)
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

			var result = await _character.FindAsync(filter);

			return await result.ToListAsync();
		}
	}
}
