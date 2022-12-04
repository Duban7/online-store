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
    public class SubcategoryRepository : ISubcategoryRepository
    {
        private readonly IMongoCollection<Subcategory> _subcategoryCollection;

        public SubcategoryRepository(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _subcategoryCollection = mongoDataBase.GetCollection<Subcategory>(GetCollectionName(typeof(Subcategory)));
        }
        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault())?.CollectionName;
        }

        public async Task<List<Subcategory>?> GetAsync(Expression<Func<Subcategory, bool>> predicant) =>
            await _subcategoryCollection.Find(predicant).ToListAsync<Subcategory>();

        public async Task CreateAsync(Subcategory newSubategory) =>
            await _subcategoryCollection.InsertOneAsync(newSubategory);

        public async Task UpdateAsync(Subcategory updatedSubcategory) =>
            await _subcategoryCollection.ReplaceOneAsync(x => x.Id == updatedSubcategory.Id, updatedSubcategory);

        public async Task RemoveAsync(string id) =>
            await _subcategoryCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<Subcategory?> GetOneByIdAsync(string id) =>
            await _subcategoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}



