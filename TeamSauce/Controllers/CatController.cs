using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamSauce.DataAccess;
using Teamsace.DataAccess.Model;

namespace TeamSauce.Controllers
{
    public class CatController : Controller
    {
        //
        // GET: /Mongo/

        [HttpGet]
        public JsonResult Index()
        {
            var mongoStore = new CategoryDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            return Json(mongoStore.GetAllCategories(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {

            var mongoStore = new CategoryDocumentStore(ConfigurationManager.AppSettings["MONGOLAB_PROD"]);

            mongoStore.CreateCategory(new Category()
            {
                categoryquestions = 
                    new List<Question>()
                        {
                            new Question()
                                {
                                    text = "Askdjf asdkfa hsdk hfasdf",
                                    imgurl = "httpwww.google.colol.gif"
                                },
                            new Question()
                                {
                                    text = "Askdjf asdkfa hsdk hfasdf",
                                    imgurl = "httpwww.google.comlol.gif"
                                }
                        },
                        categorytype = "CATS"
                        });

            return RedirectToAction("Index");
        }
    }
}
