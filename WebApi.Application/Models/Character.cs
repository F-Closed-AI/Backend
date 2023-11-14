using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Application.Models
{
	public class Character
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
		[Required]
		public int UserId { get; set; }
		[Required]
		public string? Name { get; set; }
		[Required]
		public int Age { get; set; }
		[Required]
		public string? Backstory { get; set; }
	}
}
