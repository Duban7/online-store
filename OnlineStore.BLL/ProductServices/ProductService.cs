using OnlineStore.Domain.Models;
using OnlineStore.DAL;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace OnlineStore.BLL.ProductServices
{
    public class ProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IRepository<Product> repository, ILogger<ProductService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<List<Product>> GetProducts()
        {
            _logger.LogInformation("Getting all products");
            return await _repository.GetAsync();
        }
        public async Task<List<Product>> GetProducts(Expression< Func<Product,bool>> predicante)
        {
            return await _repository.GetAsync( predicante);
        }
        public async Task<Product> GetProduct(string id)
        {
            return await _repository.GetAsync(id);
        }
    }
}
