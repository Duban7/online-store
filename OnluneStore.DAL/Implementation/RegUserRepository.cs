using MongoDB.Bson;
using MongoDB.Driver;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.Models;

namespace OnlineStore.DAL.Implementation
{
    public class RegUserRepository : IRegUserRepository
    {
        private readonly IMongoCollection<RegUser> _regUserCollection;

        public RegUserRepository(IMongoCollection<RegUser> regUserCollection)
        {
            _regUserCollection = regUserCollection;
        }

        public async Task<RegUser?> GetOneByLoginAsync(string login) =>
            await _regUserCollection.Find(regUser => regUser.Login == login).FirstOrDefaultAsync();

        public async Task<RegUser?> GetOneByLoginAndPasswordAsync(string login, string password) =>
            await _regUserCollection.Find(regUser => regUser.Login == login && regUser.Password == password).FirstOrDefaultAsync();

        public async Task<RegUser?> GetOneByIdAsync(string id) =>
            await _regUserCollection.Find(regUser => regUser.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(RegUser newRegUser) =>
            await _regUserCollection.InsertOneAsync(newRegUser);

        public async Task UpdateAsync(RegUser updatedRegUser) =>
            await _regUserCollection.ReplaceOneAsync(regUser => regUser.Id == updatedRegUser.Id, updatedRegUser);

        public async Task RemoveAsync(string id) =>
            await _regUserCollection.DeleteOneAsync(regUser => regUser.Id == id);
        public string GenerateObjectID() =>
            ObjectId.GenerateNewId().ToString();
    }
}
