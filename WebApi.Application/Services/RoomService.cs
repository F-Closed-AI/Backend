using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	}
}
