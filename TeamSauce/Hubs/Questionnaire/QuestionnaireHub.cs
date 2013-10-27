using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
        public void Complete(string questionnaireId, string data)
        {
            var questionnaireResponse = JsonConvert.DeserializeObject<QuestionnaireResponse>(data);

            using(var documentStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]))
            {
                var questionnaire = documentStore.FindQuestionnaire(questionnaireId);

                if (questionnaire.questionnaireresponses == null)
                    questionnaire.questionnaireresponses = new List<QuestionnaireResponse> { questionnaireResponse };
                else
                    questionnaire.questionnaireresponses.Add(questionnaireResponse);

                documentStore.UpsertQuestionnaire(questionnaire);                
            }
        }
    }
}