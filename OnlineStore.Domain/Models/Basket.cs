﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnlineStore.Domain.Models
{
    public class Basket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string IdUser { get; set; }
        public List<Product> Products { get; set; }
        public double TotalSum { get; set; }
    }
}
