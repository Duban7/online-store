using MongoDB.Bson;
using MongoDB.Driver;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.Models;

namespace OnlineStore.DAL.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        public CategoryRepository(IMongoCollection<Category> categoryCollection)
        {
            _categoryCollection = categoryCollection;
        }

        public async Task<List<Category>?> GetCategories() =>
            await _categoryCollection.Find(x => true).ToListAsync<Category>();

        public async Task CreateAsync(Category newCategory) =>
            await _categoryCollection.InsertOneAsync(newCategory);

        public async Task UpdateAsync(Category updatedCategory) =>
            await _categoryCollection.ReplaceOneAsync(x => x.Id == updatedCategory.Id, updatedCategory);

        public async Task RemoveAsync(string id) =>
            await _categoryCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<Category?> GetOneByIdAsync(string id) =>
            await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public string GenerateObjectID() =>
            ObjectId.GenerateNewId().ToString();
    }
}




