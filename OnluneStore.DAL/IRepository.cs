using System.Linq.Expressions;

namespace OnlineStore.DAL
{
    public interface IRepository<T> where T : class
    {
        public Task<List<T>> GetAsync();

        public Task<List<T>?> GetAsync(Expression< Func<T, bool>> predicate);
        public Task<T?> GetAsync(string id);
        public Task CreateAsync(T newBasket);
        public Task UpdateAsync(T updatedBasket);
        public Task RemoveAsync(string id);
    }
}