﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OnlineStore.Domain.Models
{
    public abstract class Model
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

    }
}
