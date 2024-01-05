using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Application.Interfaces;
using WebApi.Application.Models;

namespace WebApi.Application.Repositories
{
    public class LabelRepository
    {
        private readonly IMongoCollection<Label> _label;

        public LabelRepository(IDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _label = database.GetCollection<Label>(settings.LabelCollectionName);
        }

        public async Task<Label> CreateLabel(Label label)
        {
            await _label.InsertOneAsync(label);
            return label;
        }

        public async Task<Label> GetLabel(string id)
        {
            var filter = Builders<Label>.Filter.Eq("_id", ObjectId.Parse(id));
            return await _label.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Label>> GetAllByUserId(int userId)
        {
            var filter = Builders<Label>.Filter.Eq("UserId", userId);
            return await _label.Find(filter).ToListAsync();
        }

        public async Task<bool> UpdateLabel(string labelId, Label label)
        {
            var filter = Builders<Label>.Filter.Eq("Id", labelId);
            var update = Builders<Label>.Update
                .Set(x => x.Name, label.Name)
                .Set(x => x.Color, label.Color);

            var result = await _label.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteLabel(string labelId)
        {
            var filter = Builders<Label>.Filter.Eq("Id", labelId);
            var result = await _label.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
