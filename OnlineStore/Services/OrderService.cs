using OnlineStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace OnlineStore.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _OrderCollection;

        public OrderService(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _OrderCollection = mongoDataBase.GetCollection<Order>(OnlineStoreDataBaseSettings.Value.OrderCollectionName);
        }

        public async Task<List<Order>> GetAsync() => 
            await _OrderCollection.Find(_=>true).ToListAsync<Order>();

        public async Task<Order?> GetAsync(string id) =>
            await _OrderCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Order newOrder) =>
            await _OrderCollection.InsertOneAsync(newOrder);

        public async Task UpdateAsync(string id, Order updatedOrder) =>
            await _OrderCollection.ReplaceOneAsync(x => x.Id == id, updatedOrder);

        public async Task RemoveAsync(string id) =>
            await _OrderCollection.DeleteOneAsync(x => x.Id == id);
    }
}
