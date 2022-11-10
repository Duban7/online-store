namespace OnlineStore.DAL
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string RegUserCollectionName { get; set; } = null!;
        public string UserCollectionName { get; set; } = null!;
        public string OrderCollectionName { get; set; } = null!;
        public string BasketCollectionName { get; set; } = null!;
        public string CategoryCollectionName { get; set; } = null!;
        public string SubcategoryCollectionName { get; set; } = null!;
        public string ProductCollectionName { get; set; } = null!;
    }
}
