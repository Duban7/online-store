﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.CustomAttribute;
using OnlineStore.Domain.Models;

namespace OnlineStore.DAL.Implementation
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IMongoCollection<Basket> _basketCollection;

        public BasketRepository(IOptions<DatabaseSettings> OnlineStoreDataBaseSettings)
        {
            MongoClient mongoClient = new MongoClient(OnlineStoreDataBaseSettings.Value.ConnectionString);

            IMongoDatabase mongoDataBase = mongoClient.GetDatabase(OnlineStoreDataBaseSettings.Value.DatabaseName);

            _basketCollection = mongoDataBase.GetCollection<Basket>(GetCollectionName(typeof(Basket)));
        }
        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault())?.CollectionName;
        }

        public async Task<Basket?> GetOneByIdAsync(string id) =>
            await _basketCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Basket newBasket) =>
            await _basketCollection.InsertOneAsync(newBasket);

        public async Task UpdateAsync(Basket updatedBasket) =>
            await _basketCollection.ReplaceOneAsync(x => x.Id == updatedBasket.Id, updatedBasket);

        public async Task RemoveAsync(string id) =>
            await _basketCollection.DeleteOneAsync(x => x.Id == id);
    }
}