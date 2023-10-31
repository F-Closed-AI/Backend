using BusinessLogic;
using DataAccessMD;
using Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();


builder.Services.Configure<DatabaseSettings>(
	builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
new MongoClient(builder.Configuration.GetValue<string>("ConnectionStrings:MongoDBConnection")));

builder.Services.AddScoped<CharacterCollection, CharacterCollection>();
builder.Services.AddScoped<CharacterBL, CharacterBL>();
builder.Services.AddScoped<CharacterMD, CharacterMD>();

builder.Services.AddCors(options =>
{
	var frontendURL = configuration.GetValue<string>("FrontendURL");

	options.AddDefaultPolicy(builder =>
	{
		builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
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
