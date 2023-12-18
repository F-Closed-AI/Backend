using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Application.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? RoomId { get; set; } = ObjectId.GenerateNewId().ToString();
        [Required]
        public string Name { get; set; }
        [Required]
        public int UserId { get; set; }
        public List<string>? CharId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
