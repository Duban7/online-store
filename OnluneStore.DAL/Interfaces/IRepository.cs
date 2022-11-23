namespace OnlineStore.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<T?> GetOneByIdAsync(string id);
        public Task CreateAsync(T newModel);
        public Task UpdateAsync(T updatedModel);
        public Task RemoveAsync(string id);
    }
}