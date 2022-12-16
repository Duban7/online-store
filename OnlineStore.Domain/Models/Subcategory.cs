using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnlineStore.Domain.Models
{
    public class Subcategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
    }
}
