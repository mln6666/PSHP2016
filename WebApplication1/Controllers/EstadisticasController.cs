using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EstadisticasController : Controller
    {
        // GET: Estadisticas
        private ContextPS db = new ContextPS();

        [Authorize(Roles = "Moderador,Invitado,Administrador")]
        public ActionResult Index()
        {

            return View();
        }



        [Authorize(Roles = "Moderador,Invitado,Administrador")]
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
                if(item.FechaEvaluacionPlan>= fecha1 & item.FechaEvaluacionPlan<= fecha2)
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
                if (item.FechaEvaluacionPlan == null & item.EstadoEvaluacionPlan == Evaluacion.Pendiente)
                {
                        planespendientes++;
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