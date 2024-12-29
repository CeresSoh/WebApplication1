using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Filter;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        [AuthFilter]
        public ActionResult Index()
        {
            // Check if the user is logged in
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = Session["UserName"];
            ViewBag.IsManager = (bool)Session["IsManager"];
            return View();
        }
    }
}