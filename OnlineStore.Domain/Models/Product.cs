using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnlineStore.Domain.CustomAttribute;

namespace OnlineStore.Domain.Models
{
    [BsonCollectionAttribute("Product")]
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Counts { get; set; } 
        public double Price { get; set; }
        public string Image { get; set; }
        public Subcategory Subcategory { get; set; }
    }
}
