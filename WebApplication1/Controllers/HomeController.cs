using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class HomeController : Controller
    {
        private ContextPS db = new ContextPS();
        [AllowAnonymous]
        public ActionResult Index()
        {
            
            return View();
        }

        [Authorize(Roles = "Moderador,Invitado")]
        public ActionResult Vacio()
        {
            return View();
        }



        [AllowAnonymous]
        public ActionResult About()
        {

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
           

            return View();
        }
    }
}