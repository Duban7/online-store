using OnlineStore.Domain.CustomAttribute;

namespace OnlineStore.Domain.Models
{
    [BsonCollectionAttribute("SubCategory")]
    public class Subcategory : Model
    {
        public string Name { get; set; }
        public Category Category { get; set; }
    }
}
