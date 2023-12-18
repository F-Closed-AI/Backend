using WebApi.Application.Models;
using WebApi.Application.Repositories;

namespace WebApi.Application.Services
{
	public class RoomService
	{
		private readonly RoomRepository _roomRepository;
		public RoomService(RoomRepository roomRepository)
		{
			_roomRepository = roomRepository;
		}

		public async Task<Room> GetRoom(string id)
		{
			return await _roomRepository.GetRoom(id);
		}

        public async Task<IEnumerable<Room>> GetRooms(string roomId)
        {
            return await _roomRepository.GetRooms(roomId);
        }

        public async Task<IEnumerable<Room>> GetRoomsByUserId(int userId)
		{
			return await _roomRepository.GetRoomsByUserId(userId);
		}

		public async Task<IEnumerable<Character>> GetCharacters(string roomId)
		{
			return await _roomRepository.GetCharacters(roomId);
		}

		public async Task<Room> AddCharacter(string roomId, string charId)
		{
			return await _roomRepository.AddCharacter(roomId, charId);
		}

		public async Task<Room> RemoveCharacter(string roomId, string charId)
		{
			return await _roomRepository.RemoveCharacter(roomId, charId);
		}
	}
}
