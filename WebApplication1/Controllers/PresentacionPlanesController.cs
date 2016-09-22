using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PresentacionPlanesController : Controller
    {
        private ContextPS db = new ContextPS();

        // GET: PresentacionPlanes
        public ActionResult Index()
        {
            var presentacionPlans = db.PresentacionPlanes.Include(p => p.PS);
            return View(presentacionPlans.ToList());
        }

        


        public ActionResult HistorialPlanes(int id)
        {
            var historial = db.PSs.Include(m => m.PresentacionesPlanes).SingleOrDefault(m => m.IdPS == id);
            IEnumerable<PresentacionPlan> prueba = historial.PresentacionesPlanes.OrderByDescending(c => c.IdPresentacionPlan);
            historial.PresentacionesPlanes = prueba.ToList();
            return View(historial);
        }

        
        // GET: PresentacionPlanes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PresentacionPlan presentacionPlan = db.PresentacionPlanes.Find(id);
            if (presentacionPlan == null)
            {
                return HttpNotFound();
            }
            return View(presentacionPlan);
        }
        // GET: PresentacionPlanes/Details/5
        public ActionResult Details2(int? idps)
        {
            ContextPS db = new ContextPS();
            IEnumerable<int> query = (from c in db.PresentacionPlanes
                                      where c.IdPS == idps
                                      select c.IdPresentacionPlan);

            if (query.Count() == 0)
            {
                return HttpNotFound();
            }
            int id = query.ElementAt(query.Count()-1);
            PresentacionPlan datosplan = db.PresentacionPlanes.Find(id);

            return View("Details",datosplan);
        }


        // GET: PresentacionPlanes/Create
        public ActionResult Create(int id)
        {
            PresentacionPlan presentacionplan= new PresentacionPlan();
            presentacionplan.IdPS = id;

            PS ps = db.PSs.Find(id);


            if (ps.Estado!= Estado.Plan_Pendiente & ps.Estado != Estado.Plan_Desaprobado)
            {
                return RedirectToAction("Index", "Error", new { error = 2002 });
            }
            //ViewBag.IdPS = new SelectList(db.PSs, "IdPS", "Tutor");
            return View(presentacionplan);
        }

        // POST: PresentacionPlanes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPresentacionPlan,FechaPresentacionPlan,FechaEvaluacionPlan,EstadoEvaluacionPlan,ObservacionesPlan,IdPS")] PresentacionPlan presentacionPlan)
        {
            if (ModelState.IsValid)
            {
                db.PresentacionPlanes.Add(presentacionPlan);
                db.SaveChanges();

                PS pS = db.PSs.Find(presentacionPlan.IdPS);
                if (presentacionPlan.EstadoEvaluacionPlan==Evaluacion.Pendiente)
                    pS.Estado=Estado.Plan_Entregado;
                
                if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Aprobado)
                    pS.Estado = Estado.Plan_Aprobado;
                
                if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Desaprobado)
                    pS.Estado = Estado.Plan_Desaprobado;
                
                db.Entry(pS).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "PS", new { id = presentacionPlan.IdPS });
            }
            int id = presentacionPlan.IdPS;
            PresentacionPlan presentacionplan = new PresentacionPlan();
            presentacionplan.IdPS = id;
            return View(presentacionplan);
        }

        // GET: PresentacionPlanes/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PresentacionPlan presentacionPlan = db.PresentacionPlanes.Find(id);
            if (presentacionPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPS = new SelectList(db.PSs, "IdPS", "Tutor", presentacionPlan.IdPS);
            return View(presentacionPlan);
        }

        // POST: PresentacionPlanes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit([Bind(Include = "IdPresentacionPlan,FechaPresentacionPlan,FechaEvaluacionPlan,EstadoEvaluacionPlan,ObservacionesPlan,IdPS")] PresentacionPlan presentacionPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presentacionPlan).State = EntityState.Modified;
                db.SaveChanges();

                PS pS = db.PSs.Find(presentacionPlan.IdPS);
                if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Pendiente)
                    pS.Estado = Estado.Plan_Entregado;

                if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Aprobado)
                    pS.Estado = Estado.Plan_Aprobado;

                if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Desaprobado)
                    pS.Estado = Estado.Plan_Desaprobado;

                db.Entry(pS).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "PS", new { id = presentacionPlan.IdPS });
            }
            ViewBag.IdPS = new SelectList(db.PSs, "IdPS", "Tutor", presentacionPlan.IdPS);
            return View(presentacionPlan);
        }

        // GET: PresentacionPlanes/Edit/5
        public ActionResult EvaluarPlan(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            PresentacionPlan presentacionPlan = db.PresentacionPlanes.Find(id);
            if (presentacionPlan == null)
            {
                return HttpNotFound();
            }

            if (presentacionPlan.EstadoEvaluacionPlan != Evaluacion.Pendiente)
            {
                return RedirectToAction("Index", "Error", new { error = 2000 });
            }

            return PartialView(presentacionPlan);
        }

        // POST: PresentacionPlanes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EvaluarPlan([Bind(Include = "IdPresentacionPlan,FechaPresentacionPlan,FechaEvaluacionPlan,EstadoEvaluacionPlan,ObservacionesPlan,IdPS")] PresentacionPlan presentacionPlan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presentacionPlan).State = EntityState.Modified;
                db.SaveChanges();

                PS pS = db.PSs.Find(presentacionPlan.IdPS);
                if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Pendiente)
                    pS.Estado = Estado.Plan_Entregado;

                if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Aprobado)
                    pS.Estado = Estado.Plan_Aprobado;

                if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Desaprobado)
                    pS.Estado = Estado.Plan_Desaprobado;

                if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Rechazado)
                    pS.Estado = Estado.Plan_Rechazado;


                db.Entry(pS).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "PS", new { id = presentacionPlan.IdPS });
            }

            return PartialView(presentacionPlan);
        }


        // GET: PresentacionPlanes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PresentacionPlan presentacionPlan = db.PresentacionPlanes.Find(id);
            if (presentacionPlan == null)
            {
                return HttpNotFound();
            }
            return View(presentacionPlan);
        }

        // POST: PresentacionPlanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PresentacionPlan presentacionPlan = db.PresentacionPlanes.Find(id);
            db.PresentacionPlanes.Remove(presentacionPlan);
            db.SaveChanges();
            return RedirectToAction("Index");
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
