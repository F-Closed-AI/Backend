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
    public class RoomRepository
    {
        private IMongoCollection<Room> _room;

        public RoomRepository(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _room = database.GetCollection<Room>(settings.RoomCollectionName);
        }

        public async Task<Room> CreateRoom(Room room)
        {
            await _room.InsertOneAsync(room);
            return room;
        }
    }
}
