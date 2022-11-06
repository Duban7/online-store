using OnlineStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace OnlineStore.Services
{
    public class RegUserService
    {
        private readonly IMongoCollection<RegUser> _regUserCollection;

        public RegUserService(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _regUserCollection = mongoDataBase.GetCollection<RegUser>(OnlineStoreDataBaseSettings.Value.RegUserCollectionName);
        }

        public async Task<List<RegUser>> GetAsync() => 
            await _regUserCollection.Find(_=>true).ToListAsync<RegUser>();

        public async Task<RegUser?> GetAsync(string id) =>
            await _regUserCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(RegUser newRegUser) =>
            await _regUserCollection.InsertOneAsync(newRegUser);

        public async Task UpdateAsync(string id, RegUser updatedRegUser) =>
            await _regUserCollection.ReplaceOneAsync(x => x.Id == id, updatedRegUser);

        public async Task RemoveAsync(string id) =>
            await _regUserCollection.DeleteOneAsync(x => x.Id == id);
    }
}
