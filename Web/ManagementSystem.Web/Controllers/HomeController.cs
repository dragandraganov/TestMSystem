using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementSystem.Data;
using ekm.oledb.data;

namespace ManagementSystem.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ManagementSystemData data)
            : base(data)
        {
        }

        public ActionResult Index()
        {
            var allComments = this.Data.Comments.All(c => c.Author);

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Tasks");
            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Manual()
        {
            return View();
        }
    }
}