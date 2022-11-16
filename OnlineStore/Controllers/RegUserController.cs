using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.ProductServices;
using OnlineStore.Domain.Models;

namespace OnlineStore.Controllers
{

    [ApiController]
    [Route("")]
    public class RegUserController : ControllerBase
    {
        ProductService _productService;
        public RegUserController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("/products")]
        public async Task<ActionResult<List<Product>>> Get()
        {
           var basket = await _productService.GetProducts(p=>p.Subcategory.Category.Name == "cat1");
        
            return basket == null ? NotFound() : Ok(basket);
        }

        [HttpGet]
        [Route("/products/{id}")]
        public async Task<FileResult> GetImage(string id)
        {
            Product p = await _productService.GetProduct(id);
        
            return new FileStreamResult(System.IO.File.OpenRead(p.Image), "image/png");
        }
    }
}
