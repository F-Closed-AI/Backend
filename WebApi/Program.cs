using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebApi.Application.Interfaces;
using WebApi.Application.Models;
using WebApi.Application.Repositories;
using WebApi.Application.Services;

namespace WebApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
			var provider = builder.Services.BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
			var configuration = provider.GetRequiredService<IConfiguration>();

			builder.Services.Configure<DatabaseSettings>(
				builder.Configuration.GetSection("DatabaseSettings"));

			builder.Services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

			builder.Services.AddSingleton<IMongoClient>(s =>
			new MongoClient(builder.Configuration.GetValue<string>("ConnectionStrings:MongoDBConnection")));

			builder.Services.AddScoped<CharacterCollectionService, CharacterCollectionService>();
			builder.Services.AddScoped<CharacterService, CharacterService>();
			builder.Services.AddScoped<CharacterRepository, CharacterRepository>();

			builder.Services.AddCors(options =>
			{
				var frontendURL = configuration.GetValue<string>("FrontendURL");

				options.AddDefaultPolicy(builder =>
				{
					builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseCors();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}

}