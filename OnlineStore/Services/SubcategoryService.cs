using OnlineStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace OnlineStore.Services
{
    public class SubcategoryService
    {
        private readonly IMongoCollection<Subcategory> _SubcategoryCollection;

        public SubcategoryService(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _SubcategoryCollection = mongoDataBase.GetCollection<Subcategory>(OnlineStoreDataBaseSettings.Value.SubcategoryCollectionName);
        }

        public async Task<List<Subcategory>> GetAsync() => 
            await _SubcategoryCollection.Find(_=>true).ToListAsync<Subcategory>();

        public async Task<Subcategory?> GetAsync(string id) =>
            await _SubcategoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Subcategory newSubcategory) =>
            await _SubcategoryCollection.InsertOneAsync(newSubcategory);

        public async Task UpdateAsync(string id, Subcategory updatedSubcategory) =>
            await _SubcategoryCollection.ReplaceOneAsync(x => x.Id == id, updatedSubcategory);

        public async Task RemoveAsync(string id) =>
            await _SubcategoryCollection.DeleteOneAsync(x => x.Id == id);
    }
}
