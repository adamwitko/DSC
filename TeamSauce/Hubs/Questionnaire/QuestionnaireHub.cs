using System;
using System.Configuration;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using TeamSauce.DataAccess;
using TeamSauce.DataAccess.Model;

namespace TeamSauce.Hubs.Questionnaire
{
    [HubName("questionnaireHub")]
    public class QuestionnaireHub : Hub
    {
        public void Complete(Guid questionnaireId, string data)
        {
            var questionnaireResponse = JsonConvert.DeserializeObject<QuestionnaireResponse>(data);

            using(var documentStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]))
            {
                var questionnaire = documentStore.FindQuestionnaire(questionnaireId.ToString());

                questionnaire.questionnaireresponses.Add(questionnaireResponse);

                documentStore.UpsertQuestionnaire(questionnaire);                
            }
        }
    }
}