using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Application.Models
{
    public class Label
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Color { get; set; }
    }
}
