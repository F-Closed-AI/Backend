using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using WebApi.Application.Interfaces;
using WebApi.Application.Models;

namespace WebApi.Application.Repositories
{
    public class RoomRepository
    {
        private readonly IMongoCollection<Room> _room;
		private readonly IMongoCollection<Character> _character;
        private readonly IMongoCollection<Label> _label;

        public RoomRepository(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _room = database.GetCollection<Room>(settings.RoomCollectionName);
			_character = database.GetCollection<Character>(settings.CollectionName);
            _label = database.GetCollection<Label>(settings.LabelCollectionName);
        }

		public async Task<Room> GetRoom(string id)
		{
			var filter = Builders<Room>.Filter.And(
				Builders<Room>.Filter.Eq("_id", ObjectId.Parse(id)),
				Builders<Room>.Filter.Eq("IsDeleted", false)
			);

			return await _room.Find(filter).FirstOrDefaultAsync();
		}

        public async Task<IEnumerable<Room>> GetRooms(string roomId)
        {
            var filter = Builders<Room>.Filter.Eq("RoomId", roomId);
            var sort = Builders<Room>.Sort.Descending("DateTime");
            return await _room.Find(filter)
                .Sort(sort)
                .ToListAsync();
        }

        public async Task<List<Room>> GetRoomsByUserId(int userId)
		{
            var filter = Builders<Room>.Filter.And(
                Builders<Room>.Filter.Eq("UserId", userId),
                Builders<Room>.Filter.Eq("IsDeleted", false)
            );
            var sortDefinition = Builders<Room>.Sort.Descending("_id");
			var latestRooms =  (await _room.Find(filter)
				.Sort(sortDefinition)
				.ToListAsync())
				.GroupBy(room => room.RoomId)
				.Select(group => group.First())
				.ToDictionary(room => room.RoomId!);
			return latestRooms.Values.ToList();
        }

        public async Task<Room> CreateRoom(Room room)
        {
            await _room.InsertOneAsync(room);
            return room;
        }

		public async Task<bool> DeleteRoom(string id)
		{
            var room = await _room.Find(Builders<Room>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
            if (room == null) return false;

            var filter = Builders<Room>.Filter.Eq("RoomId", room.RoomId);
            var update = Builders<Room>.Update.Set("IsDeleted", true);

            var result = await _room.UpdateManyAsync(filter, update);
            return result.ModifiedCount > 0;
        }

		public async Task<IEnumerable<Character>> GetCharacters(string roomId)
		{
			var roomFilter = Builders<Room>.Filter.Eq("Id", roomId);
			var room = await _room.Find(roomFilter).FirstOrDefaultAsync();

			if (room == null || room.CharId?.Count == 0 || room.CharId == null)
			{
				return new List<Character>();
			}

			var characterFilter = Builders<Character>.Filter.In("Id", room.CharId);
			return await _character.Find(characterFilter).ToListAsync();
		}

		public async Task<Room> AddCharacter(string roomId, string charId)
		{
			var filter = Builders<Room>.Filter.And(
				Builders<Room>.Filter.Eq("Id", roomId),
				Builders<Room>.Filter.Eq("IsDeleted", false)
			);

			var update = Builders<Room>.Update.Push("CharId", charId);

			var options = new FindOneAndUpdateOptions<Room>
			{
				ReturnDocument = ReturnDocument.After
			};

			return await _room.FindOneAndUpdateAsync(filter, update, options);
		}

		public async Task<Room> RemoveCharacter(string roomId, string charId)
		{
			var filter = Builders<Room>.Filter.And(
				Builders<Room>.Filter.Eq("Id", roomId),
				Builders<Room>.Filter.Eq("IsDeleted", false),
				Builders<Room>.Filter.AnyEq("CharId", charId)
			);

			var roomToUpdate = await _room.Find(filter).FirstOrDefaultAsync();

			if (roomToUpdate != null)
			{
				var update = Builders<Room>.Update.Pull("CharId", charId);

				var options = new FindOneAndUpdateOptions<Room>
				{
					ReturnDocument = ReturnDocument.After
				};

				return await _room.FindOneAndUpdateAsync(filter, update, options);
			}
			else
			{
				throw new Exception($"CharId {charId} does not exist on room {roomId}.");
			}
		}

        public async Task<IEnumerable<Label>> GetLabels(string roomId)
        {
            var roomFilter = Builders<Room>.Filter.Eq("Id", roomId);
            var room = await _room.Find(roomFilter).FirstOrDefaultAsync();

            if (room == null || room.LabelId?.Count == 0 || room.LabelId == null)
            {
                return new List<Label>();
            }

            var labelFilter = Builders<Label>.Filter.In("Id", room.LabelId);
            return await _label.Find(labelFilter).ToListAsync();
        }

        public async Task<Room> AddLabel(string roomId, string labelId)
        {
            var filter = Builders<Room>.Filter.And(
                Builders<Room>.Filter.Eq("Id", roomId),
                Builders<Room>.Filter.Eq("IsDeleted", false)
            );

            var update = Builders<Room>.Update.Push("LabelId", labelId);

            var options = new FindOneAndUpdateOptions<Room>
            {
                ReturnDocument = ReturnDocument.After
            };

            return await _room.FindOneAndUpdateAsync(filter, update, options);
        }

        public async Task<Room> RemoveLabel(string roomId, string labelId)
        {
            var filter = Builders<Room>.Filter.And(
                Builders<Room>.Filter.Eq("Id", roomId),
                Builders<Room>.Filter.Eq("IsDeleted", false),
                Builders<Room>.Filter.AnyEq("LabelId", labelId)
            );

            var roomToUpdate = await _room.Find(filter).FirstOrDefaultAsync();

            if (roomToUpdate != null)
            {
                var update = Builders<Room>.Update.Pull("LabelId", labelId);

                var options = new FindOneAndUpdateOptions<Room>
                {
                    ReturnDocument = ReturnDocument.After
                };

                return await _room.FindOneAndUpdateAsync(filter, update, options);
            }
            else
            {
                throw new Exception($"LabelId {labelId} does not exist on room {roomId}.");
            }
        }
    }
}
