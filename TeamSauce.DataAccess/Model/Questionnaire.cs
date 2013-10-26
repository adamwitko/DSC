using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace TeamSauce.DataAccess.Model
{
    public class Questionnaire
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime date { get; set; }

        public List<QuestionnaireResponse> questionnaireresponses { get; set; }
    }

    public class QuestionnaireResponse
    {
        public string username { get; set; }

        public List<Rating> ratings { get; set; }
    }

    public class Rating
    {
        public string categorytype { get; set; }

        public string value { get; set; }
    }
}