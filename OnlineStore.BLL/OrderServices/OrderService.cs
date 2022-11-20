using OnlineStore.Domain.Models;
using OnlineStore.DAL;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Security.Principal;

namespace OnlineStore.BLL.OrderServices
{
    public class OrderService
    {
        private readonly IRepository<Order> _orderrepository;
        private readonly IRepository<Basket> _basketrepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IRepository<Order> repository, IRepository<Basket> basketrepository, ILogger<OrderService> logger)
        {
            _orderrepository = repository;
            _basketrepository = basketrepository;
            _logger = logger;
        }

        public async Task<List<Order>> GetOrders(Expression<Func<Order, bool>> predicante)
        {
            return await _orderrepository.GetAsync(predicante);
        }

        public async Task<Basket> GetBasket(string id)
        {
            return await _basketrepository.GetAsync(id);
        }
        public async Task<List<Basket>> GetBasket(Expression<Func<Basket, bool>> predicante)
        {
            return await _basketrepository.GetAsync(predicante);
        }

        public async Task<Order> CreateOrder(Basket newBasket)
        {
            DateTime timeNow = new DateTime();
            string id = (_orderrepository as MongoDBRepository<Order>).GenerateID();
            Order newOrder = new Order()
            {
                Id = id,
                Date = timeNow,
                IdUser = newBasket.IdUser,
                Products = newBasket.Products,
                TotalSum = newBasket.TotalSum
            };
            await _orderrepository.CreateAsync(newOrder);
            return newOrder;
        }
        
    }
}
