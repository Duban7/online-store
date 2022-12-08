using OnlineStore.Domain.Models;
using OnlineStore.DAL.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Security.Principal;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using OnlineStore.BLL.OrderServices.Exceptions;

namespace OnlineStore.BLL.OrderServices.implementation
{
    public class OrderService: IOrderService
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
            if (newBasket == null) throw new BasketNotFoundException("Basket does not exist");
            
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

        public async Task<bool> DeleteBasketProd(List<Basket> basket, string ProdId)
        {
            Basket getBasket = null;
            if (basket != null)
            {
                getBasket = basket[0];

            }
            else
            {
                throw new BasketNotFoundException();
            }
            List<Product> ProdList = new List<Product>(getBasket.Products);

            foreach (Product prod in ProdList)
            {
                if (prod.Id == ProdId)
                {
                    ProdList.Remove(prod);
                }

            }

            getBasket.Products = ProdList;
            await _basketrepository.UpdateAsync(getBasket);
            return true;

        }

    }
}
