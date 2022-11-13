using OnlineStore.Domain.CustomAttribute;

namespace OnlineStore.Domain.Models
{
    [BsonCollectionAttribute("Basket")]
    public class Basket : Model
    {
        public string IdUser { get; set; }
        public List<Product> Products { get; set; }
        public double TotalSum { get; set; }
    }
}
