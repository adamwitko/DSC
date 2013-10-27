using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using TeamSauce.DataAccess;
using TeamSauce.DataAccess.Model;

namespace TeamSauce.Controllers
{

         //
        // GET: /Mongo/
    public class QueController : Controller
    {
        //
        // GET: /Mongo/

        [HttpGet]
        public JsonResult Index()
        {
            var mongoStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            return Json(mongoStore.GetAllQuestionnaires(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AllAverage()
        {
            var mongoStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            var listOfQs = mongoStore.GetAllQuestionnaires();

            var some = listOfQs.Where(x => x.questionnaireresponses != null)
                        .Select(q =>
                                           {
                                               var catDic = new Dictionary<string, IList<int>>();
                                               var date = q.date;

                                               foreach (var el in q.questionnaireresponses)
                                               {
                                                   foreach (var subel in el.ratings)
                                                   {
                                                       if (catDic.ContainsKey(subel.categorytype))
                                                       {
                                                           catDic[subel.categorytype].Add(Int32.Parse(subel.value));
                                                       } else
                                                       {
                                                           catDic[subel.categorytype] = new List<int>(Int32.Parse(subel.value));
                                                       }
                                                   }
                                               }

                                               var properCatDic = new Dictionary<string, int>();
                                               foreach (var kv in catDic)
                                               {
                                                   properCatDic[kv.Key] =
                                                       (int)
                                                       Math.Round((float) kv.Value.Aggregate((a, e) => a + e)/
                                                                  kv.Value.Count());
                                               }

                                               return new Dictionary<string, object>
                                                          {
                                                              {"time", date.ToString("s")},
                                                              {"categories", properCatDic}
                                                          };
                                           });
            return Json(some, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Create()
        {

            var mongoStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            var questionnaire = new Questionnaire()
                                    {
                                        date = DateTime.UtcNow
                                    };
            mongoStore.UpsertQuestionnaire(questionnaire);

            return Json(questionnaire, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetQuestionnaireById(string questionnaireId)
        {
            var mongoStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            var q = mongoStore.FindQuestionnaire(questionnaireId);

            return Json(q, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        public JsonResult AddQuestionnaireResponsesToQuestionaire(string questionnaireId )
        {
            var mongoStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            var q = mongoStore.FindQuestionnaire(questionnaireId);

            if (q.questionnaireresponses == null)
                q.questionnaireresponses = new List<QuestionnaireResponse>();

            q.questionnaireresponses.Add( new QuestionnaireResponse()
                                                   {
                                                       username = "dfasdfasdfasdfasdfasdffasdfa",
                                                       ratings = new List<Rating>()
                                                                     {
                                                                         new Rating()
                                                                             {
                                                                                 categorytype = "CATS",
                                                                                 value = "5"
                                                                             }
                                                                     }
                                                   }
                                           );

            mongoStore.UpsertQuestionnaire(q);

            return Json(q, JsonRequestBehavior.AllowGet);
        }
    }
}
