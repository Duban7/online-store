using Microsoft.AspNetCore.Mvc;
using OnlineStore.Domain.Models;

namespace OnlineStore.Controllers
{
 
    [ApiController]
    [Route("")]
    public class RegUserController : ControllerBase
    {
        BLL.ProductService _productService;
        public RegUserController(BLL.ProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
           var basket = await _productService.GetProducts();
           return basket == null ? NotFound() : basket;
        }
    }
}
