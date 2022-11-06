using OnlineStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace OnlineStore.Services
{
    public class BasketService
    {
        private readonly IMongoCollection<Basket> _BasketCollection;

        public BasketService(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _BasketCollection = mongoDataBase.GetCollection<Basket>(OnlineStoreDataBaseSettings.Value.BasketCollectionName);
        }

        public async Task<List<Basket>> GetAsync() => 
            await _BasketCollection.Find(_=>true).ToListAsync<Basket>();

        public async Task<Basket?> GetAsync(string id) =>
            await _BasketCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Basket newBasket) =>
            await _BasketCollection.InsertOneAsync(newBasket);

        public async Task UpdateAsync(string id, Basket updatedBasket) =>
            await _BasketCollection.ReplaceOneAsync(x => x.Id == id, updatedBasket);

        public async Task RemoveAsync(string id) =>
            await _BasketCollection.DeleteOneAsync(x => x.Id == id);
    }
}
