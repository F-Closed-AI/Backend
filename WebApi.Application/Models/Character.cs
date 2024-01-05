using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Application.Models
{
	public class Character
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; } 
		[BsonRepresentation(BsonType.ObjectId)]
		public string? CharId { get; set; } = ObjectId.GenerateNewId().ToString();
		[Required]
		public int UserId { get; set; }
		[Required]
		public string? Prompt { get; set; }
		[Required]
		public string? Name { get; set; }
		[Required]
		public int Age { get; set; }
		[Required]
		public string? BackStory { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

	}
}
