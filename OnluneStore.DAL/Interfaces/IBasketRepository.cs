using OnlineStore.Domain.Models;

namespace OnlineStore.DAL.Interfaces
{
    public interface IBasketRepository : IRepository<Basket>
    {
        public string GenerateObjectID();
    }
}
