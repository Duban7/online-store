using OnlineStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace OnlineStore.DAL.DBCollections
{
    public class ProductCollection
    {
        private readonly IMongoCollection<Product> _ProductCollection;

        public ProductCollection(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _ProductCollection = mongoDataBase.GetCollection<Product>(OnlineStoreDataBaseSettings.Value.ProductCollectionName);
        }

        public async Task<List<Product>> GetAsync() => 
            await _ProductCollection.Find(_=>true).ToListAsync<Product>();

        public async Task<Product?> GetAsync(string id) =>
            await _ProductCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product newProduct) =>
            await _ProductCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, Product updatedProduct) =>
            await _ProductCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _ProductCollection.DeleteOneAsync(x => x.Id == id);
    }
}
