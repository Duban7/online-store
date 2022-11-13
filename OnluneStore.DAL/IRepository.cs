namespace OnlineStore.DAL
{
    public interface IRepository<T> where T : class
    {
        public Task<List<T>> GetAsync();
        public Task<List<T>?> GetAsync(Func<T, bool> predicate);
        public Task<T?> GetAsync(string id);
        public Task CreateAsync(T newBasket);
        public Task UpdateAsync(string id, T updatedBasket);
        public Task RemoveAsync(string id);
    }
}