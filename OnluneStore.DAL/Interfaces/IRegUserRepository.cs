using OnlineStore.Domain.Models;

namespace OnlineStore.DAL.Interfaces
{
    public interface IRegUserRepository : IRepository<RegUser>
    {
        public Task<RegUser?> GetOneByLoginAsync(string login);
        public  Task<RegUser?> GetOneByLoginAndPasswordAsync(string login, string password);
    }
}
