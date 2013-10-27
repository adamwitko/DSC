using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using TeamSauce.DataAccess;
using TeamSauce.DataAccess.Model;

namespace TeamSauce.Controllers
{
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
