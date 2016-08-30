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
    public class OrganizacionesController : Controller
    {
        private ContextPS db = new ContextPS();

        // GET: Organizaciones
        public ActionResult Index()
        {
            return View(db.Organizaciones.ToList());
        }

        // GET: Organizaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organizacion organizacion = db.Organizaciones.Find(id);
            if (organizacion == null)
            {
                return HttpNotFound();
            }
            return View(organizacion);
        }

        // GET: Organizaciones/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Organizaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdOrganizacion,DenominacionOrg,DireccionOrg,TelefonoOrg,DescripcionOrg")] Organizacion organizacion)
        {
            if (ModelState.IsValid)
            {
                db.Organizaciones.Add(organizacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(organizacion);
        }

        // GET: Organizaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organizacion organizacion = db.Organizaciones.Find(id);
            if (organizacion == null)
            {
                return HttpNotFound();
            }
            return View(organizacion);
        }

        // POST: Organizaciones/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdOrganizacion,DenominacionOrg,DireccionOrg,TelefonoOrg,DescripcionOrg")] Organizacion organizacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organizacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organizacion);
        }

        // GET: Organizaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organizacion organizacion = db.Organizaciones.Find(id);
            if (organizacion == null)
            {
                return HttpNotFound();
            }
            return View(organizacion);
        }

        // POST: Organizaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Organizacion organizacion = db.Organizaciones.Find(id);
            db.Organizaciones.Remove(organizacion);
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
