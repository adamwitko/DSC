using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace Teamsace.DataAccess.Model
{
    public class Category
    {
      
        public Category()
        {

        }

        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        public string categorytype { get; set; }

        public List<Question> categoryquestions { get; set; } 
    }

    public class Question
    {
        public string text { get; set; }
        public string imgurl { get; set; }
    }

}