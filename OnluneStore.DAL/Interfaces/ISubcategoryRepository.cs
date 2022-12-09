using OnlineStore.Domain.Models;
using System.Linq.Expressions;

namespace OnlineStore.DAL.Interfaces
{
    public interface ISubcategoryRepository : IRepository<Subcategory>
    {
        public string GenerateObjectID();
     
    }
}
