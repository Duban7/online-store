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

        public async Task<List<RegUser>> GetAsync() =>  await _regUserCollection.Find(_=>true).ToListAsync<RegUser>();
        
    }
}
