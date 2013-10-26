using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TeamSauce.DataAccess.Model
{
    public class UserDto
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string UserType { get; set; }

        public string TeamId { get; set; }
    }
}
