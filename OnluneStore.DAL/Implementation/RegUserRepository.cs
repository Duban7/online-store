using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.CustomAttribute;
using OnlineStore.Domain.Models;

namespace OnlineStore.DAL.Implementation
{
    public class RegUserRepository : IRegUserRepository
    {
        private readonly IMongoCollection<RegUser> _regUserCollection;

        public RegUserRepository(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _regUserCollection = mongoDataBase.GetCollection<RegUser>(GetCollectionName(typeof(RegUser)));
        }
        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault())?.CollectionName;
        }

        public async Task<RegUser?> GetOneByLoginAsync(string login) =>
            await _regUserCollection.Find(regUser => regUser.Login == login).FirstOrDefaultAsync();

        public async Task<RegUser?> GetOneByLoginAndPasswordAsync(string login, string password) =>
            await _regUserCollection.Find(regUser => regUser.Login == login && regUser.Password == password).FirstOrDefaultAsync();

        public async Task<RegUser?> GetOneByIdAsync(string id) =>
            await _regUserCollection.Find(regUser => regUser.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(RegUser newRegUser) =>
            await _regUserCollection.InsertOneAsync(newRegUser);

        public async Task UpdateAsync(RegUser updatedRegUser) =>
            await _regUserCollection.ReplaceOneAsync(regUser => regUser.Id == updatedRegUser.Id, updatedRegUser);

        public async Task RemoveAsync(string id) =>
            await _regUserCollection.DeleteOneAsync(regUser => regUser.Id == id);
    }
}
