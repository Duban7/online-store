using OnlineStore.Domain.Models;
using OnlineStore.DAL;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace OnlineStore.BLL.ProductServices
{
    public class ProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Subcategory> _subcategoryRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IRepository<Product> productRepository, IRepository<Category> categoryRepository, IRepository<Subcategory> subcategoryRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
            _categoryRepository = categoryRepository;
            _subcategoryRepository = subcategoryRepository;
        }

      
        public async Task<List<Product>> GetProducts()
        {
            _logger.LogInformation("Getting all products");

            return await _productRepository.GetAsync();
        }

        public async Task<List<Product>> GetProducts(Expression< Func<Product,bool>> predicante)
        {
            return await _productRepository.GetAsync( predicante);
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _productRepository.GetAsync(id);
        }

        public async Task<List<Category>> GetCategories()
        {
            _logger.LogInformation("Getting all categories");

            return await _categoryRepository.GetAsync();
        }
        public async Task<List<Subcategory>> GetSubcategories(string categoryName)
        {
          
            return await _subcategoryRepository.GetAsync(sub=>sub.Category.Name== categoryName);

        }
    }
}
