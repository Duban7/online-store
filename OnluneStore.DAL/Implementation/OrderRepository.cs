using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.Models;
using static MongoDB.Driver.WriteConcern;
using System.Linq.Expressions;

namespace OnlineStore.DAL.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orderCollection;

        public OrderRepository(IMongoCollection<Order> orderCollection)
        {
            _orderCollection = orderCollection;
        }

        public async Task<List<Order>?> GetAsync(Expression<Func<Order, bool>> predicant) =>
            await _orderCollection.Find(predicant).ToListAsync<Order>();
        public async Task CreateAsync(Order newOrder) =>
            await _orderCollection.InsertOneAsync(newOrder);
        public async Task UpdateAsync(Order updatedOrder) =>
            await _orderCollection.ReplaceOneAsync(x => x.Id == updatedOrder.Id, updatedOrder);

        public async Task RemoveAsync(string id) =>
            await _orderCollection.DeleteOneAsync(x => x.Id == id);
        public async Task<Order?> GetOneByIdAsync(string id) =>
            await _orderCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
    
}
