using OnlineStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.OrderServices
{
    public interface IOrderService
    {
        public Task<List<Order>> GetOrders(Expression<Func<Order, bool>> predicate);
        public Task<List<Basket>> GetBasket(Expression<Func<Basket, bool>> predicante);
        public Task<Order> CreateOrder(Basket basket);
        public Task<bool> DeleteBasketProd(List<Basket> basket, string ProdId);
    }
}
