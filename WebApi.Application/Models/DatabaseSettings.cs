using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Application.Interfaces;

namespace WebApi.Application.Models
{
	public class DatabaseSettings : IDatabaseSettings
	{
		public string CollectionName { get; set; } = string.Empty;
		public string ConnectionString { get; set; } = string.Empty;
		public string DatabaseName { get; set; } = string.Empty;
	}
}
