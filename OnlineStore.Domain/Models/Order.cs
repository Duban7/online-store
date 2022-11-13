using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnlineStore.Domain.CustomAttribute;

namespace OnlineStore.Domain.Models
{
    [BsonCollectionAttribute("Order")]
    public class Order : Model
    {
        public string IdUser { get; set; }
        public List<Product> Products { get; set; }
        public double TotalSum { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Date { get; set; }
    }
}
