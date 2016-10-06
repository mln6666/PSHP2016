using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
       
        [AllowAnonymous]
        public ActionResult Index()
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