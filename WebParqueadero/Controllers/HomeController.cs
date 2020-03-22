using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebParqueadero.Models;

namespace WebParqueadero.Controllers
{
    public class HomeController : Controller
    {
        private WebParqueaderoContext db = new WebParqueaderoContext();
        public ActionResult Index()
        {
            List<Parqueadero> ltsparqueadero = new List<Parqueadero>();
            ltsparqueadero = db.Parqueaderoes.ToList();
            return View(ltsparqueadero);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}