using OnlineStore.Domain.CustomAttribute;

namespace OnlineStore.Domain.Models
{
    [BsonCollectionAttribute("Category")]
    public class Category : Model
    {
        public string Name { get; set; }
    }
}
