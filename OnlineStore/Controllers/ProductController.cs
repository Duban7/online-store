using Microsoft.AspNetCore.Mvc;
using OnlineStore.BLL.ProductServices;
using OnlineStore.Domain.Models;

namespace OnlineStore.Controllers
{

    [ApiController]
    [Route("")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("/products")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            List<Product> allProducts = await _productService.GetProducts();

            return allProducts == null ? NotFound() : Ok(allProducts);
        }

        [HttpGet]
        [Route("/products/{id}/image")]
        public async Task<FileResult> GetImage(string id)
        {
            Product p = await _productService.GetProduct(id);

            return new FileStreamResult(System.IO.File.OpenRead(p.Image), "image/png");
        }

        [HttpGet]
        [Route("/products/{id}")]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _productService.GetProduct(id);

            return product == null ? NotFound() : Ok(product);
        }
        [HttpGet]
        [Route("/products/category")]
        public async Task<ActionResult<Product>> GetCategories()
        {
            var category = await _productService.GetCategories();

            return category == null ? NotFound() : Ok(category);
        }

        [HttpGet]
        [Route("/products/{category}/subcategory")]
        public async Task<ActionResult<Subcategory>> GetSubategories(string category)
        {
            var subcategory = await _productService.GetSubcategories(category);

            return subcategory == null ? NotFound() : Ok(subcategory);
        }
    }
}
