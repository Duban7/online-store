using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnlineStore.Domain.CustomAttribute;

namespace OnlineStore.Domain.Models
{
    [BsonCollectionAttribute("User")]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
