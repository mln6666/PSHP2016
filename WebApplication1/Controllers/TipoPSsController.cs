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
    public class TipoPSsController : Controller
    {
        private ContextPS db = new ContextPS();

        // GET: TipoPSs
        public ActionResult Index()
        {
            return View(db.TipoPSs.ToList());
        }

        // GET: TipoPSs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoPS tipoPS = db.TipoPSs.Find(id);
            if (tipoPS == null)
            {
                return HttpNotFound();
            }
            return View(tipoPS);
        }

        // GET: TipoPSs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoPSs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTipoPS,NombreTipoPS,DescripcionTipoPS")] TipoPS tipoPS)
        {
            if (ModelState.IsValid)
            {
                db.TipoPSs.Add(tipoPS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoPS);
        }

        // GET: TipoPSs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoPS tipoPS = db.TipoPSs.Find(id);
            if (tipoPS == null)
            {
                return HttpNotFound();
            }
            return View(tipoPS);
        }

        // POST: TipoPSs/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTipoPS,NombreTipoPS,DescripcionTipoPS")] TipoPS tipoPS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoPS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoPS);
        }

        // GET: TipoPSs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoPS tipoPS = db.TipoPSs.Find(id);
            if (tipoPS == null)
            {
                return HttpNotFound();
            }
            return View(tipoPS);
        }

        // POST: TipoPSs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoPS tipoPS = db.TipoPSs.Find(id);
            db.TipoPSs.Remove(tipoPS);
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
