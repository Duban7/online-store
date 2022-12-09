using MongoDB.Bson;
using MongoDB.Driver;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.Models;

namespace OnlineStore.DAL.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductRepository(IMongoCollection<Product> productCollection)
        {
            _productCollection = productCollection;
        }

        public async Task<List<Product>?> GetAllProducts() =>
            await _productCollection.Find(x=>true).ToListAsync<Product>();

        public async Task CreateAsync(Product newProduct) =>
            await _productCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(Product updatedProduct) =>
            await _productCollection.ReplaceOneAsync(x => x.Id == updatedProduct.Id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _productCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<Product?> GetOneByIdAsync(string id) =>
            await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public string GenerateObjectID() =>
            ObjectId.GenerateNewId().ToString();
    }
}



