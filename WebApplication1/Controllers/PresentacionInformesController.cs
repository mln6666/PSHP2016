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
    public class PresentacionInformesController : Controller
    {
        private ContextPS db = new ContextPS();

        // GET: PresentacionInformes
        public ActionResult Index()
        {
            var presentacionInformes = db.PresentacionInformes.Include(p => p.PS);
            return View(presentacionInformes.ToList());
        }

        public ActionResult HistorialInformes(int id)
        {
            var historial = db.PSs.Include(m => m.PresentacionesInforme).SingleOrDefault(m => m.IdPS == id);
            return View(historial);
        }

        // GET: PresentacionInformes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PresentacionInforme presentacionInforme = db.PresentacionInformes.Find(id);
            if (presentacionInforme == null)
            {
                return HttpNotFound();
            }
            return View(presentacionInforme);
        }

        public ActionResult Details2(int? idps)
        {
            ContextPS db = new ContextPS();
            IEnumerable<int> query = (from c in db.PresentacionInformes
                                      where c.IdPS == idps
                                      select c.IdPresentacionInforme);
            if (query.Count() == 0)
            {
                return HttpNotFound();
            }


            int id = query.ElementAt(0);
            PresentacionInforme datosinforme = db.PresentacionInformes.Find(id);

            return View("Details", datosinforme);
        }



        // GET: PresentacionInformes/Create
        public ActionResult Create()
        {
            ViewBag.IdPS = new SelectList(db.PSs, "IdPS", "Tutor");
            return View();
        }

        // POST: PresentacionInformes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPresentacionInforme,FechaPresentacionInforme,FechaEvaluacionInforme,EstadoEvaluacionInforme,ObservacionesInforme,IdPS")] PresentacionInforme presentacionInforme)
        {
            if (ModelState.IsValid)
            {
                db.PresentacionInformes.Add(presentacionInforme);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPS = new SelectList(db.PSs, "IdPS", "Tutor", presentacionInforme.IdPS);
            return View(presentacionInforme);
        }

        // GET: PresentacionInformes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PresentacionInforme presentacionInforme = db.PresentacionInformes.Find(id);
            if (presentacionInforme == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPS = new SelectList(db.PSs, "IdPS", "Tutor", presentacionInforme.IdPS);
            return View(presentacionInforme);
        }

        // POST: PresentacionInformes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPresentacionInforme,FechaPresentacionInforme,FechaEvaluacionInforme,EstadoEvaluacionInforme,ObservacionesInforme,IdPS")] PresentacionInforme presentacionInforme)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presentacionInforme).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdPS = new SelectList(db.PSs, "IdPS", "Tutor", presentacionInforme.IdPS);
            return View(presentacionInforme);
        }

        // GET: PresentacionInformes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PresentacionInforme presentacionInforme = db.PresentacionInformes.Find(id);
            if (presentacionInforme == null)
            {
                return HttpNotFound();
            }
            return View(presentacionInforme);
        }

        // POST: PresentacionInformes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PresentacionInforme presentacionInforme = db.PresentacionInformes.Find(id);
            db.PresentacionInformes.Remove(presentacionInforme);
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
