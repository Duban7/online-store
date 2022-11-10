using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnlineStore.Models
{
    public class RegUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }


    }
}
