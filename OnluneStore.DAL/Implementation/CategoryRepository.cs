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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        public CategoryRepository(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _categoryCollection = mongoDataBase.GetCollection<Category>(GetCollectionName(typeof(Category)));
        }
        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault())?.CollectionName;
        }

        public async Task<List<Category>?> GetAsync(Expression<Func<Category, bool>> predicant) =>
            await _categoryCollection.Find(predicant).ToListAsync<Category>();

        public async Task CreateAsync(Category newCategory) =>
            await _categoryCollection.InsertOneAsync(newCategory);

        public async Task UpdateAsync(Category updatedCategory) =>
            await _categoryCollection.ReplaceOneAsync(x => x.Id == updatedCategory.Id, updatedCategory);

        public async Task RemoveAsync(string id) =>
            await _categoryCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<Category?> GetOneByIdAsync(string id) =>
            await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}



