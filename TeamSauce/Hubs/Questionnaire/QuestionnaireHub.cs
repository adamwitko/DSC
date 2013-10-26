using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;

namespace TeamSauce.Hubs.Questionnaire
{
    [HubName("questionnaireHub")]
    public class QuestionnaireHub : Hub
    {
        public void Complete(Guid questionnaireId, string data)
        {
            var questionnaireResponse = JsonConvert.DeserializeObject<QuestionnaireResponse>(data);
        }
    }

    public class QuestionnaireResponse
    {
        public string Username { get; set; }
        public Rating[] Ratings { get; set; }
    }

    public class Rating
    {
        public CategoryType CategoryType { get; set; }
        public int Value { get; set; }
    }

    public enum CategoryType
    {
        Hunger,
        Cats
    }
}