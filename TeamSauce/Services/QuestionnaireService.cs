using System.Collections.Generic;
using System.Configuration;
using TeamSauce.DataAccess;
using TeamSauce.DataAccess.Model;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        public void Upsert(string id, QuestionnaireResponse response)
        {
            var documentStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);
            var questionnaire = documentStore.FindQuestionnaire(id);

            if (questionnaire.questionnaireresponses == null)
                questionnaire.questionnaireresponses = new List<QuestionnaireResponse> { response };
            else
                questionnaire.questionnaireresponses.Add(response);

            documentStore.UpsertQuestionnaire(questionnaire);
        }
    }
}