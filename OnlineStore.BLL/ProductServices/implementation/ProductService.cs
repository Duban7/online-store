using Microsoft.Extensions.Logging;
using OnlineStore.BLL.ProductService;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Domain.Models;


namespace OnlineStore.BLL.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubcategoryRepository _subcategoryRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository,
                            ICategoryRepository categoryRepository,
                            ISubcategoryRepository subcategoryRepository,
                            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
            _categoryRepository = categoryRepository;
            _subcategoryRepository = subcategoryRepository;
        }


        public async Task<List<Product>> GetProducts()
        {
            _logger.LogInformation("Getting all products");

            return await _productRepository.GetAllProducts();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _productRepository.GetOneByIdAsync(id);
        }

        public async Task<List<Category>> GetCategories()
        {
            _logger.LogInformation("Getting all categories");

            return await _categoryRepository.GetCategories();
        }
        public async Task<List<Subcategory>> GetSubcategories(string categoryName)
        {
            return await _subcategoryRepository.GetOneByIdAsync(sub => sub.Category.Name == categoryName);

        }
    }
}
