using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication1.Context
{
    public class ContextPS: DbContext
    {
        public System.Data.Entity.DbSet<WebApplication1.Models.PS> PSs { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Alumno> Alumnos { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Area> Areas { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Organizacion> Organizaciones { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.PresentacionInforme> PresentacionInformes { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.PresentacionPlan> PresentacionPlanes { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.TipoPS> TipoPSs { get; set; }
    }
}