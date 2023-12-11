using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Application.Models;
using WebApi.Application.Repositories;

namespace WebApi.Application.Services
{
    public class RoomCollectionService
    {

        private readonly RoomRepository _roomRepository;
        public RoomCollectionService(RoomRepository roomRepository)
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
	}
}
