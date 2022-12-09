using OnlineStore.Domain.Models;
using System.Linq.Expressions;

namespace OnlineStore.DAL.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        public string GenerateObjectID();
        public Task<List<Product>> GetAllProducts();
    }
}
