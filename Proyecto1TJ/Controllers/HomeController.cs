using Proyecto1TJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto1TJ.Controllers
{
    public class HomeController : Controller
    {
        private ProyectoFinalTJEntities1 db = new ProyectoFinalTJEntities1();
        public ActionResult Index()
        {
            if (Session["UserIDAN"] != null)
            {
                switch (db.GetRol(Session["UserIDAN"].ToString()).First().Name)
                {
                    case "Admin":
                        return RedirectToAction("Administrador");
                    case "Manager Bodega":
                        return RedirectToAction("ManagerBodega");
                    default:
                        return View();
                }
            }
            return View();
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