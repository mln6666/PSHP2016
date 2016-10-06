using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EstadisticasController : Controller
    {
        // GET: Estadisticas
        private ContextPS db = new ContextPS();

        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult Index()
        {

            return View();
        }



        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult EstadosPlanes(DateTime? fecha1,DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;
            
            //fecha2 = Convert.ToDateTime("01/02/2200").Date;


            var planespendientes = 0;
            var planesaprobados = 0;
            var planesdesaprobados = 0;
            var planesrechazados = 0;

            foreach (var item in db.PresentacionPlanes)
            {
                if(item.FechaPresentacionPlan>= fecha1 & item.FechaPresentacionPlan<= fecha2)
                { 
                if (item.EstadoEvaluacionPlan == Evaluacion.Aprobado)
                {
                    planesaprobados++;
                }
                if (item.EstadoEvaluacionPlan == Evaluacion.Desaprobado)
                {
                    planesdesaprobados++;
                }
                if (item.EstadoEvaluacionPlan == Evaluacion.Rechazado)
                {
                    planesrechazados++;
                }
                if (item.EstadoEvaluacionPlan == Evaluacion.Pendiente)
                {
                        planespendientes++;
                }
               
                }
               


            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.planesaprobados = planesaprobados;
            ViewBag.planesdesaprobados = planesdesaprobados;
            ViewBag.planesrechazados = planesrechazados;
            ViewBag.planespendientes = planespendientes;
            return View();
        }
        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult EstadosInformes(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;


            var informespendientes = 0;
            var informesaprobados = 0;
            var informesdesaprobados = 0;

            foreach (var item in db.PresentacionInformes)
            {
                if (item.FechaPresentacionInforme >= fecha1 & item.FechaPresentacionInforme <= fecha2)
                {
                    if (item.EstadoEvaluacionInforme == Evaluacion.Aprobado)
                    {
                        informesaprobados++;
                    }
                    if (item.EstadoEvaluacionInforme == Evaluacion.Desaprobado)
                    {
                        informesdesaprobados++;
                    }
                    
                    if (item.EstadoEvaluacionInforme == Evaluacion.Pendiente)
                    {
                        informespendientes++;
                    }

                }



            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.informesaprobados = informesaprobados;
            ViewBag.informesdesaprobados = informesdesaprobados;
            ViewBag.informespendientes = informespendientes;
            return View();
        }


        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult AreasPlanes(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listaareas = db.Areas.ToList();

            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.NombreArea);
            }
            int total = 0;

            for (int i = 0;i< listaareas.Count(); i++)
            {
            foreach (var item in db.PresentacionPlanes)
            {
                if (item.FechaPresentacionPlan >= fecha1 & item.FechaPresentacionPlan <= fecha2)
                {
                    if (item.PS.Area.NombreArea == listaareas.ElementAt(i).NombreArea)
                    {
                        total++;
                    }

                }
            }
                cantidades.Add(total);
                total = 0;
            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);

           

            return View();
        }

        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult AreasInformes(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listaareas = db.Areas.ToList();

            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.NombreArea);
            }
            int total = 0;

            for (int i = 0; i < listaareas.Count(); i++)
            {
                foreach (var item in db.PresentacionInformes)
                {
                    if (item.FechaPresentacionInforme >= fecha1 & item.FechaPresentacionInforme <= fecha2)
                    {
                        if (item.PS.Area.NombreArea == listaareas.ElementAt(i).NombreArea)
                        {
                            total++;
                        }

                    }
                }
                cantidades.Add(total);
                total = 0;
            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);



            return View();
        }


        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult TipospsPlanes(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listatiposps = db.TipoPSs.ToList();

            List<string> mistiposps = new List<string>();
            List<int> cantidades = new List<int>();

            foreach (var item in listatiposps)
            {
                mistiposps.Add(item.NombreTipoPS);
            }
            int total = 0;

            for (int i = 0; i < listatiposps.Count(); i++)
            {
                foreach (var item in db.PresentacionPlanes)
                {
                    if (item.FechaPresentacionPlan >= fecha1 & item.FechaPresentacionPlan <= fecha2)
                    {
                        if (item.PS.TipoPS.NombreTipoPS == listatiposps.ElementAt(i).NombreTipoPS)
                        {
                            total++;
                        }

                    }
                }
                cantidades.Add(total);
                total = 0;
            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listatiposps = JsonConvert.SerializeObject(mistiposps);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);



            return View();
        }

        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult TipospsInformes(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listatiposps = db.TipoPSs.ToList();

            List<string> mistiposps = new List<string>();
            List<int> cantidades = new List<int>();

            foreach (var item in listatiposps)
            {
                mistiposps.Add(item.NombreTipoPS);
            }
            int total = 0;

            for (int i = 0; i < listatiposps.Count(); i++)
            {
                foreach (var item in db.PresentacionInformes)
                {
                    if (item.FechaPresentacionInforme >= fecha1 & item.FechaPresentacionInforme <= fecha2)
                    {
                        if (item.PS.TipoPS.NombreTipoPS == listatiposps.ElementAt(i).NombreTipoPS)
                        {
                            total++;
                        }

                    }
                }
                cantidades.Add(total);
                total = 0;
            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listatiposps = JsonConvert.SerializeObject(mistiposps);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);



            return View();
        }

        [Authorize(Roles = "Moderador,Administrador")]
        //[HttpPost]
        public ActionResult AñoingresoPS(int fecha1, int fecha2)
        {
            

            List<int> misaños = new List<int>();
            List<int> cantidades = new List<int>();
            List<int> cantidadesps = new List<int>();



            int años = fecha2 - fecha1;

            for (int i=0;i<años+1;i++)
            {
                misaños.Add(fecha1+i);
            }
            int total = 0;
            int total1 = 0;

            for (int i = 0; i < misaños.Count(); i++)
            {
                foreach (var item in db.Alumnos)
                {
                    if (item.AñoIngreso == misaños[i])
                    {
                            total++;
                        total1 += item.PSs.Count();
                    }
                }
                cantidades.Add(total);
                cantidadesps.Add(total1);
                total = 0;
                total1 = 0;
            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaaños = JsonConvert.SerializeObject(misaños);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.cantidadesps = JsonConvert.SerializeObject(cantidadesps);




            return View();
        }


        [Authorize(Roles = "Moderador,Invitado,Administrador")]
        public ActionResult Pruebas()
        {
            var planesaprobados = 0;
            var planesdesaprobados = 0;
            var planesrechazados = 0;

            foreach (var item in db.PSs)
            {
                if (item.Estado == Estado.Plan_Aprobado)
                {
                    planesaprobados++;
                }
                if (item.Estado == Estado.Plan_Desaprobado)
                {
                    planesdesaprobados++;
                }
                if (item.Estado == Estado.Plan_Rechazado)
                {
                    planesrechazados++;
                }


            }
            ViewBag.planesaprobados = planesaprobados;
            ViewBag.planesdesaprobados = planesdesaprobados;
            ViewBag.planesrechazados = planesrechazados;
            return View();
        }
    }
}