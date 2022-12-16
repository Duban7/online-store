using MongoDB.Bson;
using MongoDB.Driver;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.Models;

namespace OnlineStore.DAL.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IMongoCollection<User> userCollection)
        {
            _userCollection = userCollection;
        }

        public async Task<User?> GetOneByIdAsync(string id) =>
            await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(User newUser) =>
            await _userCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(User updatedUser) =>
            await _userCollection.ReplaceOneAsync(x => x.Id == updatedUser.Id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _userCollection.DeleteOneAsync(x => x.Id == id);

        public string GenerateObjectID()=>
            ObjectId.GenerateNewId().ToString();
    }
}
