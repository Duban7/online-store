using System.Linq.Expressions;

namespace OnlineStore.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<List<T>?> GetAsync(Expression<Func<T, bool>> predicate);
        public Task CreateAsync(T newModel);
        public Task UpdateAsync(T updatedBasket);
        public Task RemoveAsync(string id);
    }
}
