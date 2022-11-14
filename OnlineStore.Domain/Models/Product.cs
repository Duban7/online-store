using OnlineStore.Domain.CustomAttribute;

namespace OnlineStore.Domain.Models
{
    [BsonCollectionAttribute("Product")]
    public class Product : Model
    {
        public string Name { get; set; }
        public int Count { get; set; } 
        public double Price { get; set; }
        public string Image { get; set; }
        public Subcategory Subcategory { get; set; }
    }
}
