using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using i18n;
using WebApplication1.Context;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PSController : Controller
    {
        private ContextPS db = new ContextPS();

        // GET: PS
        [Authorize(Roles = "Moderador,Invitado")]
        public ActionResult Index()
        {

            var pSs = ( from p in db.PSs
                        where p.Estado == Estado.PS_Aprobada  & p.Estado == Estado.PS_Cancelada & p.Estado == Estado.PS_Vencida & p.Estado == Estado.Plan_Rechazado
                        select p);

            var pos = db.PSs.ToList();

            foreach (var item in pSs)
            {
                pos.Remove(item);
            }



            foreach (var item in pos)
            {
                if(item.Estado==Estado.PS_Aprobada &item.NroDisposicion==null)
                pos.Remove(item);

            }


            return View(pos);
        }

        [Authorize(Roles = "Moderador,Invitado")]
        public ActionResult HistorialPS()
        {            
            var pSs = db.PSs.ToList();
            
            return View(pSs);
        }


        // GET: PS/Details/5
        [Authorize(Roles = "Moderador,Invitado")]
        public ActionResult Details(int? id, bool var)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PS pS = db.PSs.Find(id);
            
            if (pS == null)
            {
                return HttpNotFound();
            }
            ViewBag.vuelta = var;
            return View(pS);
        }

        // GET: PS/Create
        [Authorize(Roles = "Moderador")]
        public ActionResult Create()
        {
            List<Alumno> alumnos = new List<Alumno>();
            var pSs = db.PSs.ToList();
            var idmax = 7;
            PS ps;
            foreach (var item in db.Alumnos)
            {
                if (item.PSs.Count() > 0)
                {
                    idmax = item.PSs.Max(p => p.IdPS);
                    ps = db.PSs.Find(idmax);

                    if (ps.Estado == Estado.Plan_Rechazado | ps.Estado == Estado.PS_Cancelada | ps.Estado == Estado.PS_Vencida)
                    {
                        alumnos.Add(item);
                    }
                } else
                {
                    alumnos.Add(item);
                }
            }
            var band = false;
            if (alumnos.Count() == 0)
            {
                band = true;
            }
            ViewBag.band = band;
            ViewBag.alumnos = alumnos;                       
            ViewBag.areas = db.Areas.ToList();
            ViewBag.organizaciones = db.Organizaciones.ToList();
            ViewBag.IdTipoPS = new SelectList(db.TipoPSs, "IdTipoPS", "NombreTipoPS");
            return View();
        }

        // POST: PS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderador")]
        public ActionResult Create([Bind(Include = "IdPS,NroDisposicion,Tutor,TituloProyecto,CicloLectivo,Cuatrimestre,IdOrganizacion,IdArea,IdTipoPS,IdAlumno")] PS pS)
        {
            if (ModelState.IsValid)
            {
                pS.Estado=Estado.Plan_Pendiente;
                db.PSs.Add(pS);
                db.SaveChanges();
                return RedirectToAction("Details", "PS", new { id = pS.IdPS });
            }

            ////empieza copypasteada 
            List<Alumno> alumnos = new List<Alumno>();
            var pSs = db.PSs.ToList();
            var idmax = 7;
            PS ps1;
            foreach (var item in db.Alumnos)
            {
                if (item.PSs.Count() > 0)
                {
                    idmax = item.PSs.Max(p => p.IdPS);
                    ps1 = db.PSs.Find(idmax);

                    if (ps1.Estado == Estado.Plan_Rechazado | ps1.Estado == Estado.PS_Cancelada | ps1.Estado == Estado.PS_Vencida)
                    {
                        alumnos.Add(item);
                    }
                }
                else
                {
                    alumnos.Add(item);
                }
            }
            var band = false;
            if (alumnos.Count() == 0)
            {
                band = true;
            }
            ViewBag.band = band;
            ViewBag.alumnos = alumnos;
            ViewBag.areas = db.Areas.ToList();
            ViewBag.organizaciones = db.Organizaciones.ToList();
            ViewBag.IdTipoPS = new SelectList(db.TipoPSs, "IdTipoPS", "NombreTipoPS");
            ////termina copypasteada
            //ViewBag.IdAlumno = new SelectList(db.Alumnos, "IdAlumno", "NombreAlu", pS.IdAlumno);
            //ViewBag.IdArea = new SelectList(db.Areas, "IdArea", "NombreArea", pS.IdArea);
            //ViewBag.IdOrganizacion = new SelectList(db.Organizaciones, "IdOrganizacion", "DenominacionOrg", pS.IdOrganizacion);
            //ViewBag.IdTipoPS = new SelectList(db.TipoPSs, "IdTipoPS", "NombreTipoPS", pS.IdTipoPS);

            return View(pS);
        }

        // GET: PS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PS pS = db.PSs.Find(id);

            if (pS == null)
            {
                return HttpNotFound();
            }

           
                ViewBag.alumnos = db.Alumnos.ToList();
                ViewBag.areas = db.Areas.ToList();
                ViewBag.organizaciones = db.Organizaciones.ToList();
                ViewBag.IdAlumno = new SelectList(db.Alumnos, "IdAlumno", "NombreAlu", pS.IdAlumno);
                ViewBag.IdArea = new SelectList(db.Areas, "IdArea", "NombreArea", pS.IdArea);
                ViewBag.IdOrganizacion = new SelectList(db.Organizaciones, "IdOrganizacion", "DenominacionOrg", pS.IdOrganizacion);
                ViewBag.IdTipoPS = new SelectList(db.TipoPSs, "IdTipoPS", "NombreTipoPS", pS.IdTipoPS);
            

            
           

            return View(pS);
        }

        // POST: PS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPS,NroDisposicion,Tutor,TituloProyecto,CicloLectivo,Cuatrimestre,IdOrganizacion,IdArea,IdTipoPS,IdAlumno,Estado")] PS pS)
        {
            
                
            
            if (ModelState.IsValid )
            {
                db.Entry(pS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "PS", new { id = pS.IdPS });
            }

            


            ViewBag.IdAlumno = new SelectList(db.Alumnos, "IdAlumno", "NombreAlu", pS.IdAlumno);
            ViewBag.IdArea = new SelectList(db.Areas, "IdArea", "NombreArea", pS.IdArea);
            ViewBag.IdOrganizacion = new SelectList(db.Organizaciones, "IdOrganizacion", "DenominacionOrg", pS.IdOrganizacion);
            ViewBag.IdOrganizacion = new SelectList(db.TipoPSs, "IdTipoPS", "NombreTipoPS", pS.IdTipoPS);
            return View(pS);
        }

        // // // // // / // // // // // // // //
        [Authorize(Roles = "Moderador")]
        public ActionResult CreatePrimerPlan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PS pS = db.PSs.Find(id);
            if (pS == null)
            {
                return HttpNotFound();
            }

            if (pS.PresentacionesPlanes.Count != 0)
            {
                return RedirectToAction("Index", "Error", new { error = 2004 });
            }

            var pm = Mapper.Map<PS,PrimerPlanVM>(pS);

            //PrimerPlanVM pm=new PrimerPlanVM();
            //pm.IdPS = pS.IdPS;
            //pm.NroDisposicion = pS.NroDisposicion;
            //pm.Tutor = pS.Tutor;
            //pm.TituloProyecto = pS.TituloProyecto;
            //pm.CicloLectivo = pS.CicloLectivo;
            //pm.Cuatrimestre = pS.Cuatrimestre;

            //pm.IdOrganizacion = pS.IdOrganizacion;
            //pm.Organizacion = pS.Organizacion;

            //pm.IdArea = pS.IdArea;
            //pm.Area = pS.Area;

            //pm.IdTipoPS = pS.IdTipoPS;
            //pm.TipoPS = pS.TipoPS;

            //pm.Alumno = pS.Alumno;
            //pm.IdAlumno = pS.IdAlumno;

            //pm.Estado = pS.Estado;
            //pm.IdPresentacionPlan = pS.IdPS;
            //pm.FechaPresentacionPlan = pS.IdPS;
            //pm.FechaEvaluacionPlan = pS.IdPS;
            //pm.EstadoEvaluacionPlan = pS.IdPS;
            //pm.ObservacionesPlan = pS.IdPS;




            ViewBag.alumnos = db.Alumnos.ToList();
            ViewBag.areas = db.Areas.ToList();
            ViewBag.organizaciones = db.Organizaciones.ToList();
            ViewBag.IdAlumno = new SelectList(db.Alumnos, "IdAlumno", "NombreAlu", pS.IdAlumno);
            ViewBag.IdArea = new SelectList(db.Areas, "IdArea", "NombreArea", pS.IdArea);
            ViewBag.IdOrganizacion = new SelectList(db.Organizaciones, "IdOrganizacion", "DenominacionOrg", pS.IdOrganizacion);
            ViewBag.IdTipoPS = new SelectList(db.TipoPSs, "IdTipoPS", "NombreTipoPS", pS.IdTipoPS);

            return View(pm);
        }

        // POST: PS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderador")]
        public ActionResult CreatePrimerPlan([Bind(Include = "IdPS,NroDisposicion,Tutor,TituloProyecto,CicloLectivo,Cuatrimestre,IdOrganizacion,IdArea,IdTipoPS,IdAlumno,Estado,IdPresentacionPlan,FechaPresentacionPlan,FechaEvaluacionPlan,EstadoEvaluacionPlan,ObservacionesPlan")] PrimerPlanVM pm)
        {

            PS pS= new PS();
            PresentacionPlan plan=new PresentacionPlan();

            pS.IdPS = pm.IdPS;
            pS.NroDisposicion = pm.NroDisposicion;
            pS.Tutor = pm.Tutor;
            pS.TituloProyecto = pm.TituloProyecto;
            pS.CicloLectivo = pm.CicloLectivo;
            pS.Cuatrimestre = pm.Cuatrimestre;

            pS.IdOrganizacion = pm.IdOrganizacion;
            //pS.Organizacion = pm.Organizacion;

            pS.IdArea = pm.IdArea;
            //pS.IdArea = pm.IdArea;

            pS.IdTipoPS = pm.IdTipoPS;
            //pS.TipoPS = pm.TipoPS;

            //pS.Alumno = pm.Alumno;
            pS.IdAlumno = pm.IdAlumno;

            pS.Estado = pm.Estado;
            plan.IdPresentacionPlan = pm.IdPresentacionPlan;
            plan.FechaPresentacionPlan = pm.FechaPresentacionPlan;
            plan.FechaEvaluacionPlan = pm.FechaEvaluacionPlan;
            plan.EstadoEvaluacionPlan = pm.EstadoEvaluacionPlan;
            plan.ObservacionesPlan = pm.ObservacionesPlan;
            plan.IdPS = pm.IdPS;



            if (ModelState.IsValid)
            {
                db.Entry(pS).State = EntityState.Modified;
                db.SaveChanges();


                db.PresentacionPlanes.Add(plan);
                db.SaveChanges();

                PS pu = db.PSs.Find(plan.IdPS);
                if (plan.EstadoEvaluacionPlan == Evaluacion.Pendiente)
                    pu.Estado = Estado.Plan_Entregado;

                if (plan.EstadoEvaluacionPlan == Evaluacion.Aprobado)
                    pu.Estado = Estado.Plan_Aprobado;

                if (plan.EstadoEvaluacionPlan == Evaluacion.Desaprobado)
                    pu.Estado = Estado.Plan_Desaprobado;

                db.Entry(pu).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "PS", new { id = plan.IdPS });

            }

            ViewBag.alumnos = db.Alumnos.ToList();
            ViewBag.areas = db.Areas.ToList();
            ViewBag.organizaciones = db.Organizaciones.ToList();
            ViewBag.IdAlumno = new SelectList(db.Alumnos, "IdAlumno", "NombreAlu", pS.IdAlumno);
            ViewBag.IdArea = new SelectList(db.Areas, "IdArea", "NombreArea", pS.IdArea);
            ViewBag.IdOrganizacion = new SelectList(db.Organizaciones, "IdOrganizacion", "DenominacionOrg", pS.IdOrganizacion);
            ViewBag.IdOrganizacion = new SelectList(db.TipoPSs, "IdTipoPS", "NombreTipoPS", pS.IdTipoPS);
            return View(pm);
        }

        
        //GET
        public ActionResult CancelarPS(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PS ps = db.PSs.Find(id);
            if (ps == null)
            {
                return HttpNotFound();
            }
            return View(ps);
        }

        [HttpPost, ActionName("CancelarPS")]
        public ActionResult CancelarPSConfirmed(int? id)
        {
            PS ps = db.PSs.Find(id);
                        
              if (ModelState.IsValid)
              {
                  ps.Estado = Estado.PS_Cancelada;
                  db.Entry(ps).State = EntityState.Modified;
                  db.SaveChanges();
              }
              else
              {
                  ViewBag.Error = "Error: Los datos de la PS no son válidos";
              }
                          
            return RedirectToAction("Details", "PS", new { id = ps.IdPS });
        }

        //GET
        [Authorize(Roles = "Moderador")]
        public ActionResult AprobarPS(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PS ps = db.PSs.Find(id);
            if (ps == null)
            {
                return HttpNotFound();
            }
            if (ps.Estado != Estado.Informe_Aprobado)
            {
                return RedirectToAction("Index", "Error", new { error = 2005 });
            }
            return View(ps);
        }

        [HttpPost]
        [Authorize(Roles = "Moderador")]
        public ActionResult AprobarPS([Bind(Include = "IdPS,NroDisposicion")] PS pS)
        {
            PS ps=new PS();
            ps = db.PSs.Find(pS.IdPS);
            
            if (ps.Estado == Estado.Informe_Aprobado)
            {
                
                    if (pS.NroDisposicion != null)
                    {
                        ps.NroDisposicion = pS.NroDisposicion;}

                    ps.Estado = Estado.PS_Aprobada;
                    db.Entry(ps).State = EntityState.Modified;
                    db.SaveChanges();
               

            }else
            {
                ViewBag.ErrorAprobarPS = "Error: La PS seleccionada no se encuentra habilitada para ser Aprobada";
            }
            return RedirectToAction("Details", "PS", new { id = ps.IdPS });
        }

        // // // // // // // // // // // // // // //

        //GET
        [Authorize(Roles = "Moderador")]
        public ActionResult NumeroDisposicion(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PS ps = db.PSs.Find(id);
            if (ps == null)
            {
                return HttpNotFound();
            }
            if (ps.Estado != Estado.PS_Aprobada)
            {
                return RedirectToAction("Index", "Error", new { error = 2006 });
            }
            return View(ps);
        }

        [HttpPost]
        [Authorize(Roles = "Moderador")]
        public ActionResult NumeroDisposicion([Bind(Include = "IdPS,NroDisposicion")] PS pS)
        {
            PS ps = new PS();
            ps = db.PSs.Find(pS.IdPS);

            if (ps.Estado == Estado.PS_Aprobada)
            {

                if (pS.NroDisposicion != null)
                {
                    ps.NroDisposicion = pS.NroDisposicion;
                }

                db.Entry(ps).State = EntityState.Modified;
                db.SaveChanges();


            }
            
            return RedirectToAction("Details", "PS", new { id = ps.IdPS });
        }


        // // // // // // // // // // // // // // //



        // GET: PS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PS pS = db.PSs.Find(id);
            if (pS == null)
            {
                return HttpNotFound();
            }
            return View(pS);
        }

        // POST: PS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PS pS = db.PSs.Find(id);
            db.PSs.Remove(pS);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult BusquedaPS()
        {
            var alu = (from p in db.Alumnos
                       where p.PSs.Count()>0
                       select p);


            //ViewBag.alumnos = db.Alumnos.ToList();
            ViewBag.alumnos = alu;
            return View(alu);
        }

        [HttpPost]
        [Authorize(Roles = "Moderador,Invitado")]
        public ActionResult BusquedaPS([Bind(Include = "Alumno")] string alumno)
        {
            int legajo = Int32.Parse(alumno); ;
            ContextPS db = new ContextPS();
            IEnumerable<int> query = (from c in db.PSs
                                      where c.Alumno.Legajo == legajo
                                      select c.IdPS);

            if (query.Count() == 0)
            {
                return HttpNotFound();
            }
            int id = query.ElementAt(0);
            PS datosps = db.PSs.Find(id);

            return RedirectToAction("Details", "PS", new { id = id });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
