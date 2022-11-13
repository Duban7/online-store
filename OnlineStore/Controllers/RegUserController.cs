using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Models;

namespace OnlineStore.Controllers
{
 
    [ApiController]
    [Route("")]
    public class RegUserController : ControllerBase
    {
        DAL.MongoDBRepository<Basket> _BasketCollection;
        public RegUserController(DAL.MongoDBRepository<Basket> collection)
        {
            _BasketCollection = collection;
        }
        [HttpGet]
        public async Task<ActionResult<Basket?>> Get()
        {
           var basket = await _BasketCollection.GetAsync("63703352fc7378faca577c3d");
           return basket == null ? NotFound() : basket;
        }
    }
}
