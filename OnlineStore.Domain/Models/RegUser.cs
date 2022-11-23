using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using OnlineStore.Domain.CustomAttribute;

namespace OnlineStore.Domain.Models
{
    [BsonCollectionAttribute("RegUser")]
    public class RegUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }
    }
}
