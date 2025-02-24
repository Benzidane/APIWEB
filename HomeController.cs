using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Page()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        // This is the action method to handle the form submission
        [HttpPost]
        public ActionResult SubmitForm(string name, int age, bool isActive)
        {
            // You can process the form data here
            // For now, we'll simply return the values back to the view

            ViewBag.Name = name;
            ViewBag.Age = age;
            ViewBag.IsActive = isActive;

            return View("Page");
        }
    }
}
