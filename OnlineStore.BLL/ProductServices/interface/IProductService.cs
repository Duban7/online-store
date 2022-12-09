using OnlineStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.ProductService
{
    public interface IProductService
    {
        public Task<Product> GetProduct(string id);
        public Task<List<Product>> GetProducts();
        public Task<List<Category>> GetCategories();
        public Task<List<Subcategory>> GetSubcategories(string categoryName);

    }
}
