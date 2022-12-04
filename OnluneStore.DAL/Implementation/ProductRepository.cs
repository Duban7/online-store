using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.CustomAttribute;
using OnlineStore.Domain.Models;
using static MongoDB.Driver.WriteConcern;
using System.Linq.Expressions;

namespace OnlineStore.DAL.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductRepository(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _productCollection = mongoDataBase.GetCollection<Product>(GetCollectionName(typeof(Product)));
        }
        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault())?.CollectionName;
        }

        public async Task<List<Product>?> GetAsync(Expression<Func<Product, bool>> predicant) =>
            await _productCollection.Find(predicant).ToListAsync<Product>();

        public async Task CreateAsync(Product newProduct) =>
            await _productCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(Product updatedProduct) =>
            await _productCollection.ReplaceOneAsync(x => x.Id == updatedProduct.Id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _productCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<Product?> GetOneByIdAsync(string id) =>
            await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}



