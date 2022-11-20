using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.OrderServices;
using OnlineStore.Domain.Models;
using System.Security.Principal;
using static MongoDB.Driver.WriteConcern;

namespace OnlineStore.Controllers
{
    [ApiController]
    [Route("")]
    public class OrdersController : ControllerBase
    {
        OrderService _orderService;
        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

 
        [HttpGet]
        [Route("/clients/{IdUser}/orders")]
        public async Task<ActionResult<List<Order>>> Get(string IdUser)
        {
            var orders = await _orderService.GetBasket(p => p.IdUser == IdUser);

            return orders == null ? NotFound() : Ok(orders);
        }
        
        [HttpGet]
        [Route("/clients/{IdUser}/basket")]
        public async Task<ActionResult<List<Basket>>> GetUserBasket(string IdUser)
        {
            var basket  = await _orderService.GetBasket(p => p.IdUser == IdUser);

            return basket == null ? NotFound() : Ok(basket);
        }
        

        [HttpPost]
        [Route("clients/{IdUser}/basket")]
       public async Task<ActionResult<Order>> CreateNewOrder([FromBody] Basket newOrder)
        {
            Order order = await _orderService.CreateOrder(newOrder);

            return order == null ? BadRequest() : Ok(order);
        }
        
        
    }
}
