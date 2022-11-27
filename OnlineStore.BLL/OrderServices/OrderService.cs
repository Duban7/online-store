using OnlineStore.Domain.Models;
using OnlineStore.DAL.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Security.Principal;
using MongoDB.Bson;

namespace OnlineStore.BLL.OrderServices
{
    public class OrderService
    {
        private readonly IOrderRepository _orderrepository;
        private readonly IBasketRepository _basketrepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository repository, IBasketRepository basketrepository, ILogger<OrderService> logger)
        {
            _orderrepository = repository;
            _basketrepository = basketrepository;
            _logger = logger;
        }

        public async Task<List<Order>> GetOrders(Expression<Func<Order, bool>> predicante)
        {
            return await _orderrepository.GetAsync(predicante);
        }


        public async Task<List<Basket>> GetBasket(Expression<Func<Basket, bool>> predicante)
        {
            return await _basketrepository.GetAsync(predicante);
        }

        public async Task<Order> CreateOrder(Basket newBasket)
        {
            DateTime timeNow = DateTime.UtcNow;
            string id = ObjectId.GenerateNewId().ToString();
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

        public async Task<bool> DeleteBasketProd(string IdUser, string ProdId)
        {
            Basket basket = await _basketrepository.GetOneByIdAsync(IdUser);
            List<Product> ProdList = new List<Product>(basket.Products);
            for(int i = 0; i < ProdList.Count; i++)
            {
                if (ProdList[i].Id == ProdId)
                {
                    ProdList.RemoveAt(i);
                }
            }
            basket.Products = ProdList;
            await _basketrepository.UpdateAsync(basket);
            return true;
            
        }

    }
}
