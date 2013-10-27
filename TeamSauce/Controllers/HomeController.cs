using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using TeamSauce.DataAccess;
using TeamSauce.DataAccess.Model;
using TeamSauce.Hubs;
using TeamSauce.Models;

namespace TeamSauce.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHubContext _context;

        public HomeController()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<TeamSauceHub>();
        }

        public HomeController(IHubContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        public void Admin()
        {
            var documentStore = new QuestionnaireDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);
            var questionnaire = new Questionnaire { date = DateTime.UtcNow };
            documentStore.UpsertQuestionnaire(questionnaire);

            var categoryDocumentStore = new CategoryDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);
            var categories = categoryDocumentStore.GetAllCategories();

            var questionnaireForUser = new UserQuestionnaire
                {
                    id = questionnaire.Id,
                    categoryQuestions = categories.Select(c => new CategoryQuestion
                        {
                            category = c.categorytype,
                            text = c.categoryquestions[new Random().Next(0, c.categoryquestions.Count() - 1)].text
                        }).ToList()
                };

            _context.Clients.All.sentOutQuestionnaire(questionnaireForUser);
        }
    }
}
