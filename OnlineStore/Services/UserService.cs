using OnlineStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace OnlineStore.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _UserCollection;

        public UserService(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _UserCollection = mongoDataBase.GetCollection<User>(OnlineStoreDataBaseSettings.Value.UserCollectionName);
        }

        public async Task<List<User>> GetAsync() => 
            await _UserCollection.Find(_=>true).ToListAsync<User>();

        public async Task<User?> GetAsync(string id) =>
            await _UserCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _UserCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, User updatedUser) =>
            await _UserCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _UserCollection.DeleteOneAsync(x => x.Id == id);
    }
}
