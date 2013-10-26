using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using TeamSauce.DataAccess;
using TeamSauce.DataAccess.Model;
using Teamsace.DataAccess.Model;

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
        public ActionResult Create()
        {

            var mongoStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            var questionnaire = new Questionnaire()
                                    {
                                        date = DateTime.UtcNow
                                    };
            mongoStore.CreateQuestionnaire(questionnaire);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetQuestionnaireById(string Id)
        {
            var mongoStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            var q = mongoStore.FindQuestionnaire(Id);

            return Json(q, JsonRequestBehavior.AllowGet);
        }
    }
}
