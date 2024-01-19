using System.Text;
using WebApi.Application.Models;
using WebApi.Application.Repositories;
using Newtonsoft.Json;

namespace WebApi.Application.Services
{
	public class RoomService
	{
		private readonly RoomRepository _roomRepository;
		public RoomService(RoomRepository roomRepository)
		{
			_roomRepository = roomRepository;
		}

		public async Task<Room> CreateRoom(Room room)
		{
			return await _roomRepository.CreateRoom(room);
		}

		public async Task<bool> DeleteRoom(string id)
		{
			return await _roomRepository.DeleteRoom(id);
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

        public async Task<IEnumerable<Label>> GetLabels(string roomId)
        {
            return await _roomRepository.GetLabels(roomId);
        }

        public async Task<Room> AddLabel(string roomId, string labelId)
        {
            return await _roomRepository.AddLabel(roomId, labelId);
        }

        public async Task<Room> RemoveLabel(string roomId, string labelId)
        {
            return await _roomRepository.RemoveLabel(roomId, labelId);
        }

        public async Task<Conversation> CreateConversation(Character char1, Character char2, string subject)
        {
            string apiUrl = "https://api-d7b62b.stack.tryrelevance.com/latest/studios/e562e891-830c-4142-aa3c-4aca1d3eba6a/trigger_limited";

            var requestData = new
            {
                @params = new
                {
                    character_1 = char1.BackStory,
                    character_2 = char2.BackStory,
                    subject
                },
                project = "6f2b3a705849-4ac5-b8df-d50bd22fde95"
            };

            string jsonBody = JsonConvert.SerializeObject(requestData);

            using HttpClient client = new();

            try
            {
                StringContent content = new(jsonBody, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var conversation = new Conversation { Subject = subject };
                    conversation.ParseConversationContent(responseContent);
                    return conversation;
                }
                return null!;
            }
            catch (Exception)
            {
                return null!;
            }
        }
    }
}
