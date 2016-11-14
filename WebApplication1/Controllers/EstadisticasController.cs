using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
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

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult _Referencias()
        {

            return View();
        }

        [Authorize(Roles = "Moderador,Administrador")]
        public JsonResult GetVencimientos(string prueba)
        {
           
            ContextPS db = new ContextPS();
          
            ContadorVenc contadorvenc = db.ContadorVencs.ToList().FirstOrDefault();
            var pos = db.PSs.ToList();
            int vencer = 0;
            foreach (var item in pos)
            {
                if (item.Estado == Estado.Plan_Aprobado | item.Estado == Estado.Plan_Desaprobado
                    | item.Estado == Estado.Plan_Entregado | item.Estado == Estado.Informe_Entregado
                    | item.Estado == Estado.Informe_Desaprobado | item.Estado == Estado.Informe_Aprobado)
                {
                    if (item.Vencimiento < DateTime.Now)
                    {
                        vencer++;
                    }
                }

            }
            contadorvenc.CantVencAnt = contadorvenc.CantVenc;
            contadorvenc.CantVenc = vencer;

            db.Entry(contadorvenc).State = EntityState.Modified;
            db.SaveChanges();


            return Json(contadorvenc, JsonRequestBehavior.AllowGet);
        }



        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult EstadosPS()
        {

            return View();
        }

        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult EstadosPlanes()
        {

            return View();
        }
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult EstadosInformes()
        {

            return View();
        }
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult AreasPS()
        {

            return View();
        }
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult AreasPlanes()
        {

            return View();
        }
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult AreasInformes()
        {

            return View();
        }
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult TipospsPS()
        {

            return View();
        }
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult TipospsPlanes()
        {

            return View();
        }
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult TipospsInformes()
        {

            return View();
        }
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult AñoIngresoPS()
        {

            return View();
        }

        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult OrgPlanes()
        {

            return View();
        }
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult OrgInformes()
        {

            return View();
        }
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult OrgPs()
        {

            return View();
        }
       
        // // // // // // /// Pruebas
        
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult Vencimientos()
        {
            var listaPs = db.PSs.ToList();

            listaPs.RemoveAll(o => o.Estado == Estado.PS_Aprobada);
            listaPs.RemoveAll(o => o.Estado == Estado.PS_Cancelada);
            listaPs.RemoveAll(o => o.Estado == Estado.PS_Vencida);
            listaPs.RemoveAll(o => o.Estado == Estado.Plan_Rechazado);
            
            return View(listaPs);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult _PopupTipospsPS(string tipops, DateTime fecha1, DateTime fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;

            var listatiposps = db.TipoPSs.ToList();
            List<string> mistiposps = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaplanesentregados = new List<int>();
            List<int> listaplanesaprobados = new List<int>();
            List<int> listaplanesdesaprobados = new List<int>();
            List<int> listaplanesrechazados = new List<int>();
            List<int> listainformesentregados = new List<int>();
            List<int> listainformesaprobados = new List<int>();
            List<int> listainformesdesaprobados = new List<int>();
            List<int> listapssaprobadas = new List<int>();
            List<int> listapsscanceladas = new List<int>();
            List<int> listapssvencidas = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listatiposps)
            {
                mistiposps.Add(item.NombreTipoPS);
            }
            int total = 0;
            int planesentregados = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int informesentregados = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int pssaprobadas = 0;
            int psscanceladas = 0;
            int pssvencidas = 0;

            for (int i = 0; i < listatiposps.Count(); i++)
            {
                foreach (var item in db.PSs.ToList())
                {
                    bool ok = false;
                    foreach (var presentacion in item.PresentacionesPlanes)
                    {
                        if (presentacion.FechaPresentacionPlan >= fecha1 & presentacion.FechaPresentacionPlan <= fecha2)
                        {
                            ok = true;
                        }

                    }

                    if (item.PresentacionesInforme != null)
                    {
                        foreach (var presentacion in item.PresentacionesInforme)
                        {
                            if (presentacion.FechaPresentacionInforme >= fecha1 & presentacion.FechaPresentacionInforme <= fecha2)
                            {
                                ok = true;
                            }

                        }
                    }
                    if (ok)
                    {
                        pss.Add(item);

                        if (item.TipoPS.NombreTipoPS == listatiposps.ElementAt(i).NombreTipoPS)
                        {
                            total++;

                            if (item.Estado == Estado.Plan_Entregado)
                            {
                                planesentregados++;
                            }
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
                            if (item.Estado == Estado.Informe_Entregado)
                            {
                                informesentregados++;
                            }
                            if (item.Estado == Estado.Informe_Aprobado)
                            {
                                informesaprobados++;
                            }
                            if (item.Estado == Estado.Informe_Desaprobado)
                            {
                                informesdesaprobados++;
                            }
                            if (item.Estado == Estado.PS_Aprobada)
                            {
                                pssaprobadas++;
                            }
                            if (item.Estado == Estado.PS_Cancelada)
                            {
                                psscanceladas++;
                            }
                            if (item.Estado == Estado.PS_Vencida)
                            {
                                pssvencidas++;
                            }



                        }

                    }
                }
                cantidades.Add(total);

                listaplanesentregados.Add(planesentregados);
                listaplanesaprobados.Add(planesaprobados);
                listaplanesdesaprobados.Add(planesdesaprobados);
                listaplanesrechazados.Add(planesrechazados);
                listainformesentregados.Add(informesentregados);
                listainformesaprobados.Add(informesaprobados);
                listainformesdesaprobados.Add(informesdesaprobados);
                listapssaprobadas.Add(pssaprobadas);
                listapsscanceladas.Add(psscanceladas);
                listapssvencidas.Add(pssvencidas);

                total = 0;
                planesentregados = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planesrechazados = 0;
                informesentregados = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                pssaprobadas = 0;
                psscanceladas = 0;
                pssvencidas = 0;

            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listatiposps = JsonConvert.SerializeObject(mistiposps);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.tiposps = mistiposps.Count();
            ViewBag.mistiposps = mistiposps;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaplanesentregados = listaplanesentregados;
            ViewBag.listaplanesaprobados = listaplanesaprobados;
            ViewBag.listaplanesdesaprobados = listaplanesdesaprobados;
            ViewBag.listaplanesrechazados = listaplanesrechazados;
            ViewBag.listainformesentregados = listainformesentregados;
            ViewBag.listainformesaprobados = listainformesaprobados;
            ViewBag.listainformesdesaprobados = listainformesdesaprobados;
            ViewBag.listapssaprobadas = listapssaprobadas;
            ViewBag.listapsscanceladas = listapsscanceladas;
            ViewBag.listapssvencidas = listapssvencidas;

            IList<PS> pss2 = new List<PS>();

            foreach (var item in pss)
            {

                if (item.TipoPS.NombreTipoPS == tipops)
                {
                    pss2.Add(item);
                }
            }
            pss = pss2;
            pss = pss.Distinct().ToList();
            return View(pss);


        }

        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult _PopupAreasPS(string area, DateTime fecha1, DateTime fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;

            var listaareas = db.Areas.ToList();
            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaplanesentregados = new List<int>();
            List<int> listaplanesaprobados = new List<int>();
            List<int> listaplanesdesaprobados = new List<int>();
            List<int> listaplanesrechazados = new List<int>();
            List<int> listainformesentregados = new List<int>();
            List<int> listainformesaprobados = new List<int>();
            List<int> listainformesdesaprobados = new List<int>();
            List<int> listapssaprobadas = new List<int>();
            List<int> listapsscanceladas = new List<int>();
            List<int> listapssvencidas = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.NombreArea);
            }
            int total = 0;
            int planesentregados = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int informesentregados = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int pssaprobadas = 0;
            int psscanceladas = 0;
            int pssvencidas = 0;

            for (int i = 0; i < listaareas.Count(); i++)
            {
                foreach (var item in db.PSs.ToList())
                {
                    bool ok = false;
                    foreach (var presentacion in item.PresentacionesPlanes)
                    {
                        if (presentacion.FechaPresentacionPlan >= fecha1 & presentacion.FechaPresentacionPlan <= fecha2)
                        {
                            ok = true;
                        }

                    }

                    if (item.PresentacionesInforme != null)
                    {
                        foreach (var presentacion in item.PresentacionesInforme)
                        {
                            if (presentacion.FechaPresentacionInforme >= fecha1 & presentacion.FechaPresentacionInforme <= fecha2)
                            {
                                ok = true;
                            }

                        }
                    }
                    if (ok)
                    {
                        pss.Add(item);

                        if (item.Area.NombreArea == listaareas.ElementAt(i).NombreArea)
                        {
                            total++;

                            if (item.Estado == Estado.Plan_Entregado)
                            {
                                planesentregados++;
                            }
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
                            if (item.Estado == Estado.Informe_Entregado)
                            {
                                informesentregados++;
                            }
                            if (item.Estado == Estado.Informe_Aprobado)
                            {
                                informesaprobados++;
                            }
                            if (item.Estado == Estado.Informe_Desaprobado)
                            {
                                informesdesaprobados++;
                            }
                            if (item.Estado == Estado.PS_Aprobada)
                            {
                                pssaprobadas++;
                            }
                            if (item.Estado == Estado.PS_Cancelada)
                            {
                                psscanceladas++;
                            }
                            if (item.Estado == Estado.PS_Vencida)
                            {
                                pssvencidas++;
                            }



                        }

                    }
                }
                cantidades.Add(total);

                listaplanesentregados.Add(planesentregados);
                listaplanesaprobados.Add(planesaprobados);
                listaplanesdesaprobados.Add(planesdesaprobados);
                listaplanesrechazados.Add(planesrechazados);
                listainformesentregados.Add(informesentregados);
                listainformesaprobados.Add(informesaprobados);
                listainformesdesaprobados.Add(informesdesaprobados);
                listapssaprobadas.Add(pssaprobadas);
                listapsscanceladas.Add(psscanceladas);
                listapssvencidas.Add(pssvencidas);

                total = 0;
                planesentregados = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planesrechazados = 0;
                informesentregados = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                pssaprobadas = 0;
                psscanceladas = 0;
                pssvencidas = 0;

            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misareas.Count();
            ViewBag.misareas = misareas;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaplanesentregados = listaplanesentregados;
            ViewBag.listaplanesaprobados = listaplanesaprobados;
            ViewBag.listaplanesdesaprobados = listaplanesdesaprobados;
            ViewBag.listaplanesrechazados = listaplanesrechazados;
            ViewBag.listainformesentregados = listainformesentregados;
            ViewBag.listainformesaprobados = listainformesaprobados;
            ViewBag.listainformesdesaprobados = listainformesdesaprobados;
            ViewBag.listapssaprobadas = listapssaprobadas;
            ViewBag.listapsscanceladas = listapsscanceladas;
            ViewBag.listapssvencidas = listapssvencidas;

            IList<PS> pss2 = new List<PS>();

            foreach (var item in pss)
            {

                if (item.Area.NombreArea == area)
                {
                    pss2.Add(item);
                }
            }
            pss = pss2;
            pss = pss.Distinct().ToList();


            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult _PopupOrgPs(string area, DateTime fecha1, DateTime fecha2)
        {


            var listaareas = db.Organizaciones.ToList();
            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaplanesentregados = new List<int>();
            List<int> listaplanesaprobados = new List<int>();
            List<int> listaplanesdesaprobados = new List<int>();
            List<int> listaplanesrechazados = new List<int>();
            List<int> listainformesentregados = new List<int>();
            List<int> listainformesaprobados = new List<int>();
            List<int> listainformesdesaprobados = new List<int>();
            List<int> listapssaprobadas = new List<int>();
            List<int> listapsscanceladas = new List<int>();
            List<int> listapssvencidas = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.DenominacionOrg);
            }
            int total = 0;
            int planesentregados = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int informesentregados = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int pssaprobadas = 0;
            int psscanceladas = 0;
            int pssvencidas = 0;

            for (int i = 0; i < listaareas.Count(); i++)
            {
                foreach (var item in db.PSs.ToList())
                {
                    bool ok = false;
                    foreach (var presentacion in item.PresentacionesPlanes)
                    {
                        if (presentacion.FechaPresentacionPlan >= fecha1 & presentacion.FechaPresentacionPlan <= fecha2)
                        {
                            ok = true;
                        }

                    }

                    if (item.PresentacionesInforme != null)
                    {
                        foreach (var presentacion in item.PresentacionesInforme)
                        {
                            if (presentacion.FechaPresentacionInforme >= fecha1 & presentacion.FechaPresentacionInforme <= fecha2)
                            {
                                ok = true;
                            }

                        }
                    }
                    if (ok)
                    {
                        pss.Add(item);

                        if (item.Organizacion.DenominacionOrg == listaareas.ElementAt(i).DenominacionOrg)
                        {
                            total++;

                            if (item.Estado == Estado.Plan_Entregado)
                            {
                                planesentregados++;
                            }
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
                            if (item.Estado == Estado.Informe_Entregado)
                            {
                                informesentregados++;
                            }
                            if (item.Estado == Estado.Informe_Aprobado)
                            {
                                informesaprobados++;
                            }
                            if (item.Estado == Estado.Informe_Desaprobado)
                            {
                                informesdesaprobados++;
                            }
                            if (item.Estado == Estado.PS_Aprobada)
                            {
                                pssaprobadas++;
                            }
                            if (item.Estado == Estado.PS_Cancelada)
                            {
                                psscanceladas++;
                            }
                            if (item.Estado == Estado.PS_Vencida)
                            {
                                pssvencidas++;
                            }



                        }

                    }
                }
                cantidades.Add(total);

                listaplanesentregados.Add(planesentregados);
                listaplanesaprobados.Add(planesaprobados);
                listaplanesdesaprobados.Add(planesdesaprobados);
                listaplanesrechazados.Add(planesrechazados);
                listainformesentregados.Add(informesentregados);
                listainformesaprobados.Add(informesaprobados);
                listainformesdesaprobados.Add(informesdesaprobados);
                listapssaprobadas.Add(pssaprobadas);
                listapsscanceladas.Add(psscanceladas);
                listapssvencidas.Add(pssvencidas);

                total = 0;
                planesentregados = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planesrechazados = 0;
                informesentregados = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                pssaprobadas = 0;
                psscanceladas = 0;
                pssvencidas = 0;

            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misareas.Count();
            ViewBag.misareas = misareas;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaplanesentregados = listaplanesentregados;
            ViewBag.listaplanesaprobados = listaplanesaprobados;
            ViewBag.listaplanesdesaprobados = listaplanesdesaprobados;
            ViewBag.listaplanesrechazados = listaplanesrechazados;
            ViewBag.listainformesentregados = listainformesentregados;
            ViewBag.listainformesaprobados = listainformesaprobados;
            ViewBag.listainformesdesaprobados = listainformesdesaprobados;
            ViewBag.listapssaprobadas = listapssaprobadas;
            ViewBag.listapsscanceladas = listapsscanceladas;
            ViewBag.listapssvencidas = listapssvencidas;

            IList<PS> pss2 = new List<PS>();

            foreach (var item in pss)
            {

                if (item.Organizacion.DenominacionOrg == area)
                {
                    pss2.Add(item);
                }
            }
            pss = pss2;
            pss = pss.Distinct().ToList();



            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult _PopupAreasPlanes(string area,DateTime fecha1, DateTime fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listaareas = db.Areas.ToList();

            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            List<int> listarechazados = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.NombreArea);
            }
            int total = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int planespendientes = 0;

            for (int i = 0; i < listaareas.Count(); i++)
            {
                foreach (var item in db.PresentacionPlanes)
                {
                    if (item.FechaPresentacionPlan >= fecha1 & item.FechaPresentacionPlan <= fecha2)
                    {
                        pss.Add(item.PS);
                        if (item.PS.Area.NombreArea == listaareas.ElementAt(i).NombreArea)
                        {
                            total++;

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
                }
                cantidades.Add(total);
                listaaprobados.Add(planesaprobados);
                listadesaprobados.Add(planesdesaprobados);
                listapendientes.Add(planespendientes);
                listarechazados.Add(planesrechazados);
                total = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planespendientes = 0;
                planesrechazados = 0;
            }
          
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misareas.Count();
            ViewBag.misareas = misareas;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;
            ViewBag.listarechazados = listarechazados;

            IList<PS> pss2 = new List<PS>();

            foreach (var item in pss)
            {

                if (item.Area.NombreArea == area)
                {
                    pss2.Add(item);
                }
            }
            pss = pss2;
            pss = pss.Distinct().ToList();

          
                


            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult _PopupOrgPlanes(string organizacion, DateTime fecha1, DateTime fecha2)
        {

            var listaOrgs = db.Organizaciones.ToList();

            List<string> misorgs = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            List<int> listarechazados = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaOrgs)
            {
                misorgs.Add(item.DenominacionOrg);
            }
            int total = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int planespendientes = 0;

            for (int i = 0; i < listaOrgs.Count(); i++)
            {
                foreach (var item in db.PresentacionPlanes)
                {
                    if (item.FechaPresentacionPlan >= fecha1 & item.FechaPresentacionPlan <= fecha2)
                    {
                        pss.Add(item.PS);
                        if (item.PS.Organizacion.DenominacionOrg == listaOrgs.ElementAt(i).DenominacionOrg)
                        {
                            total++;

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
                }
                cantidades.Add(total);
                listaaprobados.Add(planesaprobados);
                listadesaprobados.Add(planesdesaprobados);
                listapendientes.Add(planespendientes);
                listarechazados.Add(planesrechazados);
                total = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planespendientes = 0;
                planesrechazados = 0;
            }
          
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misorgs);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misorgs.Count();
            ViewBag.misareas = misorgs;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;
            ViewBag.listarechazados = listarechazados;

            IList<PS> pss2 = new List<PS>();

            foreach (var item in pss)
            {

                if (item.Organizacion.DenominacionOrg == organizacion)
                {
                    pss2.Add(item);
                }
            }
            pss = pss2;
            pss = pss.Distinct().ToList();

            return View(pss);
        }


        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult _PopupTipospsPlanes(string tipops, DateTime fecha1, DateTime fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listatiposps = db.TipoPSs.ToList();

            List<string> mistiposps = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            List<int> listarechazados = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listatiposps)
            {
                mistiposps.Add(item.NombreTipoPS);
            }
            int total = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int planespendientes = 0;

            for (int i = 0; i < listatiposps.Count(); i++)
            {
                foreach (var item in db.PresentacionPlanes)
                {
                    if (item.FechaPresentacionPlan >= fecha1 & item.FechaPresentacionPlan <= fecha2)
                    {
                        pss.Add(item.PS);
                        if (item.PS.TipoPS.NombreTipoPS == listatiposps.ElementAt(i).NombreTipoPS)
                        {
                            total++;
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
                }
                cantidades.Add(total);
                listaaprobados.Add(planesaprobados);
                listadesaprobados.Add(planesdesaprobados);
                listapendientes.Add(planespendientes);
                listarechazados.Add(planesrechazados);
                total = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planespendientes = 0;
                planesrechazados = 0;
            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listatiposps = JsonConvert.SerializeObject(mistiposps);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.tiposps = mistiposps.Count();
            ViewBag.mistiposps = mistiposps;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;
            ViewBag.listarechazados = listarechazados;

            IList<PS> pss2 = new List<PS>();

            foreach (var item in pss)
            {

                if (item.TipoPS.NombreTipoPS == tipops)
                {
                    pss2.Add(item);
                }
            }
            pss = pss2;
            pss = pss.Distinct().ToList();
            return View(pss);
        }


        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult _PopupAreasInformes(string area, DateTime fecha1, DateTime fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listaareas = db.Areas.ToList();

            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.NombreArea);
            }
            int total = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int informespendientes = 0;

            for (int i = 0; i < listaareas.Count(); i++)
            {
                foreach (var item in db.PresentacionInformes)
                {
                    if (item.FechaPresentacionInforme >= fecha1 & item.FechaPresentacionInforme <= fecha2)
                    {
                        pss.Add(item.PS);
                        if (item.PS.Area.NombreArea == listaareas.ElementAt(i).NombreArea)
                        {
                            total++;
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
                }
                cantidades.Add(total);
                listaaprobados.Add(informesaprobados);
                listadesaprobados.Add(informesdesaprobados);
                listapendientes.Add(informespendientes);
                total = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                informespendientes = 0;


            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misareas.Count();
            ViewBag.misareas = misareas;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;

            IList<PS> pss2 = new List<PS>();

            foreach (var item in pss)
            {

                if (item.Area.NombreArea == area)
                {
                    pss2.Add(item);
                }
            }
            pss = pss2;
            pss = pss.Distinct().ToList();

            return View(pss);
        }


        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult _PopupTipospsInformes(string tipops, DateTime fecha1, DateTime fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listatiposps = db.TipoPSs.ToList();

            List<string> mistiposps = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            IList<PS> pss = new List<PS>();
            foreach (var item in listatiposps)
            {
                mistiposps.Add(item.NombreTipoPS);
            }
            int total = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int informespendientes = 0;

            for (int i = 0; i < listatiposps.Count(); i++)
            {
                foreach (var item in db.PresentacionInformes)
                {
                    if (item.FechaPresentacionInforme >= fecha1 & item.FechaPresentacionInforme <= fecha2)
                    {
                        pss.Add(item.PS);
                        if (item.PS.TipoPS.NombreTipoPS == listatiposps.ElementAt(i).NombreTipoPS)
                        {
                            total++;
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
                }
                cantidades.Add(total);
                listaaprobados.Add(informesaprobados);
                listadesaprobados.Add(informesdesaprobados);
                listapendientes.Add(informespendientes);
                total = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                informespendientes = 0;
            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listatiposps = JsonConvert.SerializeObject(mistiposps);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.tiposps = mistiposps.Count();
            ViewBag.mistiposps = mistiposps;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;

            IList<PS> pss2 = new List<PS>();

            foreach (var item in pss)
            {

                if (item.TipoPS.NombreTipoPS == tipops)
                {
                    pss2.Add(item);
                }
            }
            pss = pss2;
            pss = pss.Distinct().ToList();
            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult _PopupOrgInformes(string organizacion, DateTime fecha1, DateTime fecha2)
        {

            var listaareas = db.Organizaciones.ToList();

            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.DenominacionOrg);
            }
            int total = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int informespendientes = 0;

            for (int i = 0; i < listaareas.Count(); i++)
            {
                foreach (var item in db.PresentacionInformes)
                {
                    if (item.FechaPresentacionInforme >= fecha1 & item.FechaPresentacionInforme <= fecha2)
                    {
                        pss.Add(item.PS);
                        if (item.PS.Organizacion.DenominacionOrg == listaareas.ElementAt(i).DenominacionOrg)
                        {
                            total++;
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
                }
                cantidades.Add(total);
                listaaprobados.Add(informesaprobados);
                listadesaprobados.Add(informesdesaprobados);
                listapendientes.Add(informespendientes);
                total = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                informespendientes = 0;


            }
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misareas.Count();
            ViewBag.misareas = misareas;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;

            IList<PS> pss2 = new List<PS>();

            foreach (var item in pss)
            {

                if (item.Organizacion.DenominacionOrg == organizacion)
                {
                    pss2.Add(item);
                }
            }
            pss = pss2;
            pss = pss.Distinct().ToList();
            return View(pss);
        }
        // // / // / // / /// // Fin pruebas

        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _GraficoEstadosPS(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;

            List<string> estadosstring2 = new List<string>();
            estadosstring2.Add("P.Ent.");
            estadosstring2.Add("P.Ap.");
            estadosstring2.Add("P.Des.");
            estadosstring2.Add("P.Rech.");
            estadosstring2.Add("I.Ent.");
            estadosstring2.Add("I.Ap.");
            estadosstring2.Add("I.Des.");
            estadosstring2.Add("Ps.Ap.");
            estadosstring2.Add("Ps.Canc.");
            estadosstring2.Add("Ps.Venc.");
            List<string> estadosstring = new List<string>();
            estadosstring.Add("Plan_Entregado");
            estadosstring.Add("Plan_Aprobado");
            estadosstring.Add("Plan_Desaprobado");
            estadosstring.Add("Plan_Rechazado");
            estadosstring.Add("Informe_Entregado");
            estadosstring.Add("Informe_Aprobado");
            estadosstring.Add("Informe_Desaprobado");
            estadosstring.Add("PS_Aprobada");
            estadosstring.Add("PS_Cancelada");
            estadosstring.Add("PS_Vencida");
            List<Estado> misestados = new List<Estado>();
            misestados.Add(Estado.Plan_Entregado);
            misestados.Add(Estado.Plan_Aprobado);
            misestados.Add(Estado.Plan_Desaprobado);
            misestados.Add(Estado.Plan_Rechazado);
            misestados.Add(Estado.Informe_Entregado);
            misestados.Add(Estado.Informe_Aprobado);
            misestados.Add(Estado.Informe_Desaprobado);
            misestados.Add(Estado.PS_Aprobada);
            misestados.Add(Estado.PS_Cancelada);
            misestados.Add(Estado.PS_Vencida);
            var listaestados = misestados;

            List<int> cantidades = new List<int>();
           
            IList<PS> pss = new List<PS>();

           
            int total = 0;
            

            for (int i = 0; i < listaestados.Count(); i++)
            {
                foreach (var item in db.PSs.ToList())
                {
                    bool ok = false;
                    if (item.PresentacionesInforme == null | item.PresentacionesInforme.Count()==0)
                    {
                        if (item.PresentacionesPlanes.LastOrDefault().FechaPresentacionPlan >= fecha1 & item.PresentacionesPlanes.LastOrDefault().FechaPresentacionPlan <= fecha2)
                        {
                            ok = true;
                        }


                    }
                    else
                    {
                        if (item.PresentacionesInforme.LastOrDefault().FechaPresentacionInforme >= fecha1 & item.PresentacionesInforme.LastOrDefault().FechaPresentacionInforme <= fecha2)
                        {
                            ok = true;
                        }
                    }
                   

                   
                    if (ok)
                    {
                        pss.Add(item);

                        if (item.Estado == listaestados.ElementAt(i))
                        {
                            total++;
                        }

                    }
                }
                cantidades.Add(total);

              
                total = 0;
               

            }
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaestados = JsonConvert.SerializeObject(estadosstring2);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.estados = misestados.Count();
            ViewBag.misestados = misestados;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)

            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _GraficoEstadosPlanes(DateTime? fecha1,DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;
            
            //fecha2 = Convert.ToDateTime("01/02/2200").Date;


            var planespendientes = 0;
            var planesaprobados = 0;
            var planesdesaprobados = 0;
            var planesrechazados = 0;
            IList<PS> pss = new List<PS>();


            foreach (var item in db.PresentacionPlanes)
            {
                if(item.FechaPresentacionPlan>= fecha1 & item.FechaPresentacionPlan<= fecha2)
                { 
                    pss.Add(item.PS);

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
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.planesaprobados = planesaprobados;
            ViewBag.planesdesaprobados = planesdesaprobados;
            ViewBag.planesrechazados = planesrechazados;
            ViewBag.planespendientes = planespendientes;


            return View(pss);
        }
        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _GraficoEstadosInformes(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;


            var informespendientes = 0;
            var informesaprobados = 0;
            var informesdesaprobados = 0;
            IList<PS> pss = new List<PS>();
            foreach (var item in db.PresentacionInformes)
            {
                if (item.FechaPresentacionInforme >= fecha1 & item.FechaPresentacionInforme <= fecha2)
                {
                    pss.Add(item.PS);
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
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.informesaprobados = informesaprobados;
            ViewBag.informesdesaprobados = informesdesaprobados;
            ViewBag.informespendientes = informespendientes;
            return View(pss);
        }

        //List<string> estadosps = new List<string>();
        //estadosps.Add(Estado.Plan_Pendiente.ToString());
        //    estadosps.Add(Estado.Plan_Entregado.ToString());
        //    estadosps.Add(Estado.Plan_Aprobado.ToString());
        //    estadosps.Add(Estado.Plan_Desaprobado.ToString());
        //    estadosps.Add(Estado.Plan_Rechazado.ToString());
        //    estadosps.Add(Estado.Informe_Entregado.ToString());
        //    estadosps.Add(Estado.Informe_Aprobado.ToString());
        //    estadosps.Add(Estado.Informe_Desaprobado.ToString());
        //    estadosps.Add(Estado.PS_Aprobada.ToString());
        //    estadosps.Add(Estado.PS_Cancelada.ToString());
        //    estadosps.Add(Estado.PS_Vencida.ToString());

        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _GraficoAreasPS(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;

            var listaareas = db.Areas.ToList();
            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaplanesentregados = new List<int>(); 
            List<int> listaplanesaprobados = new List<int>(); 
            List<int> listaplanesdesaprobados = new List<int>();
            List<int> listaplanesrechazados = new List<int>();
            List<int> listainformesentregados = new List<int>();
            List<int> listainformesaprobados = new List<int>();
            List<int> listainformesdesaprobados = new List<int>();
            List<int> listapssaprobadas = new List<int>();
            List<int> listapsscanceladas = new List<int>();
            List<int> listapssvencidas = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.NombreArea);
            }
            int total = 0;
            int planesentregados = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int informesentregados = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int pssaprobadas = 0;
            int psscanceladas = 0;
            int pssvencidas = 0;

            for (int i = 0; i < listaareas.Count(); i++)
            {
                foreach (var item in db.PSs.ToList())
                {
                    bool ok = false;
                    foreach (var presentacion in item.PresentacionesPlanes)
                    {
                        if (presentacion.FechaPresentacionPlan >= fecha1 & presentacion.FechaPresentacionPlan <= fecha2)
                        {
                            ok = true;
                        }
                        
                    }

                    if (item.PresentacionesInforme != null)
                    {
                        foreach (var presentacion in item.PresentacionesInforme)
                        {
                            if (presentacion.FechaPresentacionInforme >= fecha1 & presentacion.FechaPresentacionInforme <= fecha2)
                            {
                                ok = true;
                            }

                        }
                    }
                    if (ok)
                    {
                        pss.Add(item);
                        
                        if (item.Area.NombreArea == listaareas.ElementAt(i).NombreArea)
                        {
                            total++;

                            if (item.Estado == Estado.Plan_Entregado)
                            {
                                planesentregados++;
                            }
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
                            if (item.Estado == Estado.Informe_Entregado)
                            {
                                informesentregados++;
                            }
                            if (item.Estado == Estado.Informe_Aprobado)
                            {
                                informesaprobados++;
                            }
                            if (item.Estado == Estado.Informe_Desaprobado)
                            {
                                informesdesaprobados++;
                            }
                            if (item.Estado == Estado.PS_Aprobada)
                            {
                                pssaprobadas++;
                            }
                            if (item.Estado == Estado.PS_Cancelada)
                            {
                                psscanceladas++;
                            }
                            if (item.Estado == Estado.PS_Vencida)
                            {
                                pssvencidas++;
                            }



                        }

                    }
                }
                cantidades.Add(total);
                
                listaplanesentregados.Add(planesentregados);
                listaplanesaprobados.Add(planesaprobados);
                listaplanesdesaprobados.Add(planesdesaprobados);
                listaplanesrechazados.Add(planesrechazados);
                listainformesentregados.Add(informesentregados);
                listainformesaprobados.Add(informesaprobados);
                listainformesdesaprobados.Add(informesdesaprobados);
                listapssaprobadas.Add(pssaprobadas);
                listapsscanceladas.Add(psscanceladas);
                listapssvencidas.Add(pssvencidas);
               
                total = 0;
                planesentregados = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planesrechazados = 0;
                informesentregados = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                pssaprobadas = 0;
                psscanceladas = 0;
                pssvencidas = 0;
                
            }
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misareas.Count();
            ViewBag.misareas = misareas;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaplanesentregados = listaplanesentregados;
            ViewBag.listaplanesaprobados = listaplanesaprobados;
            ViewBag.listaplanesdesaprobados = listaplanesdesaprobados;
            ViewBag.listaplanesrechazados = listaplanesrechazados;
            ViewBag.listainformesentregados = listainformesentregados;
            ViewBag.listainformesaprobados = listainformesaprobados;
            ViewBag.listainformesdesaprobados = listainformesdesaprobados;
            ViewBag.listapssaprobadas = listapssaprobadas;
            ViewBag.listapsscanceladas = listapsscanceladas;
            ViewBag.listapssvencidas = listapssvencidas;
         



            return View(pss);
        }


        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _OrgPs(DateTime? fecha1, DateTime? fecha2)
        {
            

            var listaareas = db.Organizaciones.ToList();
            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaplanesentregados = new List<int>();
            List<int> listaplanesaprobados = new List<int>();
            List<int> listaplanesdesaprobados = new List<int>();
            List<int> listaplanesrechazados = new List<int>();
            List<int> listainformesentregados = new List<int>();
            List<int> listainformesaprobados = new List<int>();
            List<int> listainformesdesaprobados = new List<int>();
            List<int> listapssaprobadas = new List<int>();
            List<int> listapsscanceladas = new List<int>();
            List<int> listapssvencidas = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.DenominacionOrg);
            }
            int total = 0;
            int planesentregados = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int informesentregados = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int pssaprobadas = 0;
            int psscanceladas = 0;
            int pssvencidas = 0;

            for (int i = 0; i < listaareas.Count(); i++)
            {
                foreach (var item in db.PSs.ToList())
                {
                    bool ok = false;
                    foreach (var presentacion in item.PresentacionesPlanes)
                    {
                        if (presentacion.FechaPresentacionPlan >= fecha1 & presentacion.FechaPresentacionPlan <= fecha2)
                        {
                            ok = true;
                        }

                    }

                    if (item.PresentacionesInforme != null)
                    {
                        foreach (var presentacion in item.PresentacionesInforme)
                        {
                            if (presentacion.FechaPresentacionInforme >= fecha1 & presentacion.FechaPresentacionInforme <= fecha2)
                            {
                                ok = true;
                            }

                        }
                    }
                    if (ok)
                    {
                        pss.Add(item);

                        if (item.Organizacion.DenominacionOrg == listaareas.ElementAt(i).DenominacionOrg)
                        {
                            total++;

                            if (item.Estado == Estado.Plan_Entregado)
                            {
                                planesentregados++;
                            }
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
                            if (item.Estado == Estado.Informe_Entregado)
                            {
                                informesentregados++;
                            }
                            if (item.Estado == Estado.Informe_Aprobado)
                            {
                                informesaprobados++;
                            }
                            if (item.Estado == Estado.Informe_Desaprobado)
                            {
                                informesdesaprobados++;
                            }
                            if (item.Estado == Estado.PS_Aprobada)
                            {
                                pssaprobadas++;
                            }
                            if (item.Estado == Estado.PS_Cancelada)
                            {
                                psscanceladas++;
                            }
                            if (item.Estado == Estado.PS_Vencida)
                            {
                                pssvencidas++;
                            }



                        }

                    }
                }
                cantidades.Add(total);

                listaplanesentregados.Add(planesentregados);
                listaplanesaprobados.Add(planesaprobados);
                listaplanesdesaprobados.Add(planesdesaprobados);
                listaplanesrechazados.Add(planesrechazados);
                listainformesentregados.Add(informesentregados);
                listainformesaprobados.Add(informesaprobados);
                listainformesdesaprobados.Add(informesdesaprobados);
                listapssaprobadas.Add(pssaprobadas);
                listapsscanceladas.Add(psscanceladas);
                listapssvencidas.Add(pssvencidas);

                total = 0;
                planesentregados = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planesrechazados = 0;
                informesentregados = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                pssaprobadas = 0;
                psscanceladas = 0;
                pssvencidas = 0;

            }
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misareas.Count();
            ViewBag.misareas = misareas;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaplanesentregados = listaplanesentregados;
            ViewBag.listaplanesaprobados = listaplanesaprobados;
            ViewBag.listaplanesdesaprobados = listaplanesdesaprobados;
            ViewBag.listaplanesrechazados = listaplanesrechazados;
            ViewBag.listainformesentregados = listainformesentregados;
            ViewBag.listainformesaprobados = listainformesaprobados;
            ViewBag.listainformesdesaprobados = listainformesdesaprobados;
            ViewBag.listapssaprobadas = listapssaprobadas;
            ViewBag.listapsscanceladas = listapsscanceladas;
            ViewBag.listapssvencidas = listapssvencidas;




            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _GraficoAreasPlanes(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listaareas = db.Areas.ToList();

            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            List<int> listarechazados = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.NombreArea);
            }
            int total = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int planespendientes = 0;

            for (int i = 0;i< listaareas.Count(); i++)
            {
            foreach (var item in db.PresentacionPlanes)
            {
                if (item.FechaPresentacionPlan >= fecha1 & item.FechaPresentacionPlan <= fecha2)
                {
                        pss.Add(item.PS);
                        if (item.PS.Area.NombreArea == listaareas.ElementAt(i).NombreArea)
                    {
                        total++;

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
            }
                cantidades.Add(total);
                listaaprobados.Add(planesaprobados);
                listadesaprobados.Add(planesdesaprobados);
                listapendientes.Add(planespendientes);
                listarechazados.Add(planesrechazados);
                total = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planespendientes = 0;
                planesrechazados = 0;
            }
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misareas.Count();
            ViewBag.misareas = misareas;
            ViewBag.miscantidades =cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;
            ViewBag.listarechazados = listarechazados;

           

            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _OrgPlanes(DateTime? fecha1, DateTime? fecha2)
        {
            
            var listaOrgs = db.Organizaciones.ToList();

            List<string> misorgs = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            List<int> listarechazados = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaOrgs)
            {
                misorgs.Add(item.DenominacionOrg);
            }
            int total = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int planespendientes = 0;

            for (int i = 0; i < listaOrgs.Count(); i++)
            {
                foreach (var item in db.PresentacionPlanes)
                {
                    if (item.FechaPresentacionPlan >= fecha1 & item.FechaPresentacionPlan <= fecha2)
                    {
                        pss.Add(item.PS);
                        if (item.PS.Organizacion.DenominacionOrg == listaOrgs.ElementAt(i).DenominacionOrg)
                        {
                            total++;

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
                }
                cantidades.Add(total);
                listaaprobados.Add(planesaprobados);
                listadesaprobados.Add(planesdesaprobados);
                listapendientes.Add(planespendientes);
                listarechazados.Add(planesrechazados);
                total = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planespendientes = 0;
                planesrechazados = 0;
            }
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misorgs);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misorgs.Count();
            ViewBag.misareas = misorgs;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;
            ViewBag.listarechazados = listarechazados;



            return View(pss);
        }

       


        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _GraficoAreasInformes(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listaareas = db.Areas.ToList();

            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.NombreArea);
            }
            int total = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int informespendientes = 0;

            for (int i = 0; i < listaareas.Count(); i++)
            {
                foreach (var item in db.PresentacionInformes)
                {
                    if (item.FechaPresentacionInforme >= fecha1 & item.FechaPresentacionInforme <= fecha2)
                    {
                        pss.Add(item.PS);
                        if (item.PS.Area.NombreArea == listaareas.ElementAt(i).NombreArea)
                        {
                            total++;
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
                }
                cantidades.Add(total);
                listaaprobados.Add(informesaprobados);
                listadesaprobados.Add(informesdesaprobados);
                listapendientes.Add(informespendientes);
                total = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                informespendientes = 0;
                
               
            }
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misareas.Count();
            ViewBag.misareas = misareas;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;


            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _OrgInformes(DateTime? fecha1, DateTime? fecha2)
        {
            
            var listaareas = db.Organizaciones.ToList();

            List<string> misareas = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listaareas)
            {
                misareas.Add(item.DenominacionOrg);
            }
            int total = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int informespendientes = 0;

            for (int i = 0; i < listaareas.Count(); i++)
            {
                foreach (var item in db.PresentacionInformes)
                {
                    if (item.FechaPresentacionInforme >= fecha1 & item.FechaPresentacionInforme <= fecha2)
                    {
                        pss.Add(item.PS);
                        if (item.PS.Organizacion.DenominacionOrg == listaareas.ElementAt(i).DenominacionOrg)
                        {
                            total++;
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
                }
                cantidades.Add(total);
                listaaprobados.Add(informesaprobados);
                listadesaprobados.Add(informesdesaprobados);
                listapendientes.Add(informespendientes);
                total = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                informespendientes = 0;


            }
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listaareas = JsonConvert.SerializeObject(misareas);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.areas = misareas.Count();
            ViewBag.misareas = misareas;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;


            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _GraficoTipospsPS(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;

            var listatiposps = db.TipoPSs.ToList();
            List<string> mistiposps = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaplanesentregados = new List<int>();
            List<int> listaplanesaprobados = new List<int>();
            List<int> listaplanesdesaprobados = new List<int>();
            List<int> listaplanesrechazados = new List<int>();
            List<int> listainformesentregados = new List<int>();
            List<int> listainformesaprobados = new List<int>();
            List<int> listainformesdesaprobados = new List<int>();
            List<int> listapssaprobadas = new List<int>();
            List<int> listapsscanceladas = new List<int>();
            List<int> listapssvencidas = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listatiposps)
            {
                mistiposps.Add(item.NombreTipoPS);
            }
            int total = 0;
            int planesentregados = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int informesentregados = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int pssaprobadas = 0;
            int psscanceladas = 0;
            int pssvencidas = 0;

            for (int i = 0; i < listatiposps.Count(); i++)
            {
                foreach (var item in db.PSs.ToList())
                {
                    bool ok = false;
                    foreach (var presentacion in item.PresentacionesPlanes)
                    {
                        if (presentacion.FechaPresentacionPlan >= fecha1 & presentacion.FechaPresentacionPlan <= fecha2)
                        {
                            ok = true;
                        }

                    }

                    if (item.PresentacionesInforme != null)
                    {
                        foreach (var presentacion in item.PresentacionesInforme)
                        {
                            if (presentacion.FechaPresentacionInforme >= fecha1 & presentacion.FechaPresentacionInforme <= fecha2)
                            {
                                ok = true;
                            }

                        }
                    }
                    if (ok)
                    {
                        pss.Add(item);

                        if (item.TipoPS.NombreTipoPS == listatiposps.ElementAt(i).NombreTipoPS)
                        {
                            total++;

                            if (item.Estado == Estado.Plan_Entregado)
                            {
                                planesentregados++;
                            }
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
                            if (item.Estado == Estado.Informe_Entregado)
                            {
                                informesentregados++;
                            }
                            if (item.Estado == Estado.Informe_Aprobado)
                            {
                                informesaprobados++;
                            }
                            if (item.Estado == Estado.Informe_Desaprobado)
                            {
                                informesdesaprobados++;
                            }
                            if (item.Estado == Estado.PS_Aprobada)
                            {
                                pssaprobadas++;
                            }
                            if (item.Estado == Estado.PS_Cancelada)
                            {
                                psscanceladas++;
                            }
                            if (item.Estado == Estado.PS_Vencida)
                            {
                                pssvencidas++;
                            }



                        }

                    }
                }
                cantidades.Add(total);

                listaplanesentregados.Add(planesentregados);
                listaplanesaprobados.Add(planesaprobados);
                listaplanesdesaprobados.Add(planesdesaprobados);
                listaplanesrechazados.Add(planesrechazados);
                listainformesentregados.Add(informesentregados);
                listainformesaprobados.Add(informesaprobados);
                listainformesdesaprobados.Add(informesdesaprobados);
                listapssaprobadas.Add(pssaprobadas);
                listapsscanceladas.Add(psscanceladas);
                listapssvencidas.Add(pssvencidas);

                total = 0;
                planesentregados = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planesrechazados = 0;
                informesentregados = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                pssaprobadas = 0;
                psscanceladas = 0;
                pssvencidas = 0;

            }
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listatiposps = JsonConvert.SerializeObject(mistiposps);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.tiposps = mistiposps.Count();
            ViewBag.mistiposps = mistiposps;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaplanesentregados = listaplanesentregados;
            ViewBag.listaplanesaprobados = listaplanesaprobados;
            ViewBag.listaplanesdesaprobados = listaplanesdesaprobados;
            ViewBag.listaplanesrechazados = listaplanesrechazados;
            ViewBag.listainformesentregados = listainformesentregados;
            ViewBag.listainformesaprobados = listainformesaprobados;
            ViewBag.listainformesdesaprobados = listainformesdesaprobados;
            ViewBag.listapssaprobadas = listapssaprobadas;
            ViewBag.listapsscanceladas = listapsscanceladas;
            ViewBag.listapssvencidas = listapssvencidas;




            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _GraficoTipospsPlanes(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listatiposps = db.TipoPSs.ToList();

            List<string> mistiposps = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            List<int> listarechazados = new List<int>();
            IList<PS> pss = new List<PS>();

            foreach (var item in listatiposps)
            {
                mistiposps.Add(item.NombreTipoPS);
            }
            int total = 0;
            int planesaprobados = 0;
            int planesdesaprobados = 0;
            int planesrechazados = 0;
            int planespendientes = 0;

            for (int i = 0; i < listatiposps.Count(); i++)
            {
                foreach (var item in db.PresentacionPlanes)
                {
                    if (item.FechaPresentacionPlan >= fecha1 & item.FechaPresentacionPlan <= fecha2)
                    {
                        pss.Add(item.PS);
                        if (item.PS.TipoPS.NombreTipoPS == listatiposps.ElementAt(i).NombreTipoPS)
                        {
                            total++;
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
                }
                cantidades.Add(total);
                listaaprobados.Add(planesaprobados);
                listadesaprobados.Add(planesdesaprobados);
                listapendientes.Add(planespendientes);
                listarechazados.Add(planesrechazados);
                total = 0;
                planesaprobados = 0;
                planesdesaprobados = 0;
                planespendientes = 0;
                planesrechazados = 0;
            }
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listatiposps = JsonConvert.SerializeObject(mistiposps);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.tiposps = mistiposps.Count();
            ViewBag.mistiposps = mistiposps;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;
            ViewBag.listarechazados = listarechazados;


            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        [HttpPost]
        public ActionResult _GraficoTipospsInformes(DateTime? fecha1, DateTime? fecha2)
        {
            //fecha1 = Convert.ToDateTime("01/02/0001").Date;

            //fecha2 = Convert.ToDateTime("01/02/2200").Date;
            var listatiposps = db.TipoPSs.ToList();

            List<string> mistiposps = new List<string>();
            List<int> cantidades = new List<int>();
            List<int> listaaprobados = new List<int>();
            List<int> listadesaprobados = new List<int>();
            List<int> listapendientes = new List<int>();
            IList<PS> pss = new List<PS>();
            foreach (var item in listatiposps)
            {
                mistiposps.Add(item.NombreTipoPS);
            }
            int total = 0;
            int informesaprobados = 0;
            int informesdesaprobados = 0;
            int informespendientes = 0;

            for (int i = 0; i < listatiposps.Count(); i++)
            {
                foreach (var item in db.PresentacionInformes)
                {
                    if (item.FechaPresentacionInforme >= fecha1 & item.FechaPresentacionInforme <= fecha2)
                    {
                        pss.Add(item.PS);
                        if (item.PS.TipoPS.NombreTipoPS == listatiposps.ElementAt(i).NombreTipoPS)
                        {
                            total++;
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
                }
                cantidades.Add(total);
                listaaprobados.Add(informesaprobados);
                listadesaprobados.Add(informesdesaprobados);
                listapendientes.Add(informespendientes);
                total = 0;
                informesaprobados = 0;
                informesdesaprobados = 0;
                informespendientes = 0;
            }
            pss = pss.Distinct().ToList();
            ViewBag.fechadesde = fecha1;
            ViewBag.fechahasta = fecha2;
            ViewBag.listatiposps = JsonConvert.SerializeObject(mistiposps);
            ViewBag.cantidades = JsonConvert.SerializeObject(cantidades);
            ViewBag.tiposps = mistiposps.Count();
            ViewBag.mistiposps = mistiposps;
            ViewBag.miscantidades = cantidades;//total de planes /informes segun el metodo (no se ocupa por ahora)
            ViewBag.listaaprobados = listaaprobados;
            ViewBag.listadesaprobados = listadesaprobados;
            ViewBag.listapendientes = listapendientes;

            return View(pss);
        }

        [Authorize(Roles = "Moderador,Administrador")]
        //[HttpPost]
        public ActionResult _GraficoAñoIngresoPS(int fecha1, int fecha2)
        {
            

            List<int> misaños = new List<int>();
            List<int> cantidades = new List<int>();
            List<int> cantidadesps = new List<int>();
            IList<PS> pss= new List<PS>();


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

                        foreach (var mips in item.PSs)
                        {
                            pss.Add(mips);
                        }

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




            return View(pss);
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