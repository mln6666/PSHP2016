using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            CreateRoles(db);
            CreateSuperuser(db);
            AddPermisionsToSuperuser(db);
            db.Dispose();

            AreaRegistration.RegisterAllAreas();            
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        

          
        
        private void AddPermisionsToSuperuser(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));


            var user = userManager.FindByName("admin@utnfrre.com");

            if (!userManager.IsInRole(user.Id, "Ver"))
            {
                userManager.AddToRole(user.Id, "Ver");
            }
            if (!userManager.IsInRole(user.Id, "Crear"))
            {
                userManager.AddToRole(user.Id, "Crear");
            }
            if (!userManager.IsInRole(user.Id, "Editar"))
            {
                userManager.AddToRole(user.Id, "Editar");
            }
            if (!userManager.IsInRole(user.Id, "Eliminar"))
            {
                userManager.AddToRole(user.Id, "Eliminar");
            }
            if (!userManager.IsInRole(user.Id, "Administrador"))
            {
                userManager.AddToRole(user.Id, "Administrador");
            }


        }

        private void CreateSuperuser(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.FindByName("admin@utnfrre.com");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "admin@utnfrre.com",
                    Email = "admin@utnfrre.com"
                };
                userManager.Create(user, "Abc123.");
            }

        }

        private void CreateRoles(ApplicationDbContext db)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            if (!roleManager.RoleExists("Administrador"))
            {
                roleManager.Create(new IdentityRole("Administrador"));
            }

            if (!roleManager.RoleExists("Ver"))
            {
                roleManager.Create(new IdentityRole("Ver"));
            }

            if (!roleManager.RoleExists("Editar"))
            {
                roleManager.Create(new IdentityRole("Editar"));
            }

            if (!roleManager.RoleExists("Crear"))
            {
                roleManager.Create(new IdentityRole("Crear"));
            }

            if (!roleManager.RoleExists("Eliminar"))
            {
                roleManager.Create(new IdentityRole("Eliminar"));
            }

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            int error = httpException != null ? httpException.GetHttpCode() : 0;

            Server.ClearError();
            Response.Redirect(String.Format("~/Error/?error={0}", error, exception.Message));
        }


    }
}
