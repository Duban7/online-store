using OnlineStore.Domain.CustomAttribute;

namespace OnlineStore.Domain.Models
{
    [BsonCollectionAttribute("Subcategory")]
    public class Subcategory : Model
    {
        public string Name { get; set; }
        public Category Category { get; set; }
    }
}
