using OnlineStore.Domain.Models;

namespace OnlineStore.DAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public string GenerateObjectID();
    }
}
