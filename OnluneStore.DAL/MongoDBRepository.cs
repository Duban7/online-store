using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnlineStore.Domain.CustomAttribute;
using OnlineStore.Domain.Models;

namespace OnlineStore.DAL
{
    public class MongoDBRepository <TModel> : IRepository<TModel> where TModel : Model
    {
        private readonly IMongoCollection<TModel> _TModelCollection;

        public MongoDBRepository(DatabaseSettings OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.DatabaseName);

            _TModelCollection = mongoDataBase.GetCollection<TModel>(GetCollectionName(typeof(TModel)));
        }
        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault())?.CollectionName;
        }
        public async Task<List<TModel>> GetAsync() =>
            await _TModelCollection.Find(_ => true).ToListAsync<TModel>();
        public async Task<List<TModel>?> GetAsync(Func<TModel,bool> predicant) =>
            await _TModelCollection.Find(_ => true).ToListAsync<TModel>();
        public async Task<TModel?> GetAsync(string id) =>
            await _TModelCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(TModel newTModel) =>
            await _TModelCollection.InsertOneAsync(newTModel);

        public async Task UpdateAsync(string id, TModel updatedTModel) =>
            await _TModelCollection.ReplaceOneAsync(x => x.Id == id, updatedTModel);

        public async Task RemoveAsync(string id) =>
            await _TModelCollection.DeleteOneAsync(x => x.Id == id);
    }
}
