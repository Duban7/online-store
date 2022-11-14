using OnlineStore.Domain.Models;
using OnlineStore.DAL;
using Microsoft.Extensions.Options;

namespace OnlineStore.BLL
{
    public class ProductService
    {
        private readonly IRepository<Product> _repository;

        public ProductService(IOptions<DatabaseSettings> settings)
        {
            _repository = new MongoDBRepository<Product>(settings.Value);
        }
        public async Task<List<Product>> GetProducts()
        {
            return await _repository.GetAsync();
        }
    }
}
