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
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.App_Start;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            ContextPS dc = new ContextPS();
            ApplicationDbContext db = new ApplicationDbContext();
            CreateRoles(db);
            CreateSuperuser(db);
            AddPermisionsToSuperuser(db);
            db.Dispose();
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());
            AreaRegistration.RegisterAllAreas();            
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (dc.TipoPSs.Count() == 0 | dc.TipoPSs == null)
            {
                TipoPS tipo1 = new TipoPS();
                TipoPS tipo2 = new TipoPS();
                TipoPS tipo3 = new TipoPS();
                TipoPS tipo4 = new TipoPS();

                tipo1.NombreTipoPS = "Pasantía / Laboral";
                tipo1.DescripcionTipoPS = "Una Relación Laboral o Pasantía rentada en empresas del medio.";
                tipo2.NombreTipoPS = "Proyecto Final";
                tipo2.DescripcionTipoPS = "Desarrollo de un Proyecto Final que haya sido acordado mediante un convenio específico con terceros, entes y/o empresas privadas o públicas, grupos de investigación y/o desarrollo tecnológico y de servicios.";
                tipo3.NombreTipoPS = "Investigación / Desarrollo";
                tipo3.DescripcionTipoPS = "Participación activa en Grupos de Investigación, Desarrollo y/o Aplicación Tecnológica y de Servicios a terceros realizados por Grupos de Investigación o desarrollo tecnológico y de servicios acreditados, perteneciente a instituciones de nivel académico reconocido.";
                tipo4.NombreTipoPS = "ONG / Instituciones / Empresas";
                tipo4.DescripcionTipoPS = "Desarrollar tareas de ingeniería en el ambito de ONG y/o Instituciones y/o Empresas Productivas o de Servicios públicas o privadas.";
                dc.TipoPSs.Add(tipo1);
                dc.TipoPSs.Add(tipo2);
                dc.TipoPSs.Add(tipo3);
                dc.TipoPSs.Add(tipo4);
                dc.SaveChanges();
            }

            if (dc.Areas.Count() == 0 | dc.Areas == null)
            {
                Area area1 = new Area();
                Area area2 = new Area();
                Area area3 = new Area();
                Area area4 = new Area();

                area1.NombreArea = "Desarrollo";
                area1.DescripcionArea = "- - -";
                area2.NombreArea = "Testing";
                area2.DescripcionArea = "- - -";
                area3.NombreArea = "Redes";
                area3.DescripcionArea = "- - -";
                area4.NombreArea = "Calidad / QA";
                area4.DescripcionArea = "- - -";
                dc.Areas.Add(area1);
                dc.Areas.Add(area2);
                dc.Areas.Add(area3);
                dc.Areas.Add(area4);
                dc.SaveChanges();
            }




        }

        

          
        
        private void AddPermisionsToSuperuser(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));


            var user = userManager.FindByName("admin@utnfrre.com");

          
            if (!userManager.IsInRole(user.Id, "Editar"))
            {
                userManager.AddToRole(user.Id, "Editar");

            }
            if (!userManager.IsInRole(user.Id, "Eliminar"))
            {
                userManager.AddToRole(user.Id, "Eliminar");
                }
            if (!userManager.IsInRole(user.Id, "Moderador"))
            {
                userManager.AddToRole(user.Id, "Moderador");
            }

           
            if (!userManager.IsInRole(user.Id, "Administrador"))
            {
                userManager.AddToRole(user.Id, "Administrador");
            }
            if (!userManager.IsInRole(user.Id, "Invitado"))
            {
                userManager.AddToRole(user.Id, "Invitado");
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

          

            if (!roleManager.RoleExists("Editar"))
            {
                roleManager.Create(new IdentityRole("Editar"));
            }

          
            if (!roleManager.RoleExists("Eliminar"))
            {
                roleManager.Create(new IdentityRole("Eliminar"));
            }
            if (!roleManager.RoleExists("Moderador"))
            {
                roleManager.Create(new IdentityRole("Moderador"));
            }
            if (!roleManager.RoleExists("Invitado"))
            {
                roleManager.Create(new IdentityRole("Invitado"));
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
