﻿using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TeamSauce.DataAccess.Model
{
    public class TeamMessageDto
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        public string Message { get; set; }
        public string Sender { get; set; }
        public DateTime Time { get; set; }
    }
}
