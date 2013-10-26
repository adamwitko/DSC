using System;
using System.Configuration;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using TeamSauce.DataAccess;
using TeamSauce.Hubs.Questionnaire.Data;

namespace TeamSauce.Hubs.Questionnaire
{
    [HubName("questionnaireHub")]
    public class QuestionnaireHub : Hub
    {
        public void Complete(Guid questionnaireId, string data)
        {
            var questionnaireResponse = JsonConvert.DeserializeObject<QuestionnaireResponse>(data);

            var documentStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);
        }
    }
}