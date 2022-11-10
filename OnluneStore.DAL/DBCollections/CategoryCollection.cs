using OnlineStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace OnlineStore.DAL.DBCollections
{
    public class CategoryCollection
    {
        private readonly IMongoCollection<Category> _CategoryCollection;

        public CategoryCollection(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _CategoryCollection = mongoDataBase.GetCollection<Category>(OnlineStoreDataBaseSettings.Value.CategoryCollectionName);
        }

        public async Task<List<Category>> GetAsync() => 
            await _CategoryCollection.Find(_=>true).ToListAsync<Category>();

        public async Task<Category?> GetAsync(string id) =>
            await _CategoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Category newCategory) =>
            await _CategoryCollection.InsertOneAsync(newCategory);

        public async Task UpdateAsync(string id, Category updatedCategory) =>
            await _CategoryCollection.ReplaceOneAsync(x => x.Id == id, updatedCategory);

        public async Task RemoveAsync(string id) =>
            await _CategoryCollection.DeleteOneAsync(x => x.Id == id);
    }
}
