using MongoDB.Bson;
using MongoDB.Driver;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.Models;

namespace OnlineStore.DAL.Implementation
{
    public class SubcategoryRepository : ISubcategoryRepository
    {

        private readonly IMongoCollection<Subcategory> _subcategoryCollection;

        public SubcategoryRepository(IMongoCollection<Subcategory> subcategoryCollection)
        {
            _subcategoryCollection = subcategoryCollection;
        }

        public async Task<Subcategory?> GetOneByIdAsync(string id) =>
            await _subcategoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Subcategory newSubcategory) =>
            await _subcategoryCollection.InsertOneAsync(newSubcategory);

        public async Task UpdateAsync(Subcategory updatedSubcategory) =>
            await _subcategoryCollection.ReplaceOneAsync(x => x.Id == updatedSubcategory.Id, updatedSubcategory);

        public async Task RemoveAsync(string id) =>
            await _subcategoryCollection.DeleteOneAsync(x => x.Id == id);

        public string GenerateObjectID() =>
            ObjectId.GenerateNewId().ToString();
    }
}




