using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TeamSauce.DataAccess;
using TeamSauce.Services.Interfaces;

namespace TeamSauce.Services
{
    public class QuestionnaireResultsService : IQuestionnaireResultService
    {
        public IEnumerable<IDictionary<string, object>> GetData()
        {
            var mongoStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            var questionnaires = mongoStore.GetAllQuestionnaires();

            return questionnaires.Where(x => x.questionnaireresponses != null)
                .Select(q =>
                    {
                        var categoryDictionary = new Dictionary<string, IList<int>>();
                        var date = q.date;

                        foreach (var subel in q.questionnaireresponses.SelectMany(el => el.ratings))
                        {
                            if (categoryDictionary.ContainsKey(subel.categorytype))
                            {
                                categoryDictionary[subel.categorytype].Add(Int32.Parse(subel.value));
                            }
                            else
                            {
                                categoryDictionary[subel.categorytype] = new List<int>(Int32.Parse(subel.value));
                            }
                        }

                        var properCatDic = new Dictionary<string, int>();
                        foreach (var kv in categoryDictionary)
                        {
                            if (!kv.Value.Any())
                            {
                                properCatDic[kv.Key] = 0;
                                continue;
                            }
                            properCatDic[kv.Key] = (int)Math.Round((float)kv.Value.Aggregate((a, e) => a + e) / kv.Value.Count());
                        }

                        return new Dictionary<string, object>
                            {
                                {"time", date.ToString("s")},
                                {"categories", properCatDic}
                            };
                    });
        }
    }
}