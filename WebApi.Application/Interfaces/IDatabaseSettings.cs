namespace WebApi.Application.Interfaces
{
	public interface IDatabaseSettings
	{
		string CollectionName { get; set; }
		string RoomCollectionName { get; set; }
		string ConnectionString { get; set; }
		string DatabaseName { get; set; }
	}
}
