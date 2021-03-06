﻿using System;
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
    public class AreasController : Controller
    {
        private ContextPS db = new ContextPS();

        // GET: Areas
        [Authorize(Roles = "Moderador,Invitado,Administrador")]
        public ActionResult Index()
        {
            return View(db.Areas.ToList());
        }

        // GET: Areas/Details/5
        [Authorize(Roles = "Moderador,Invitado,Administrador")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // GET: Areas/Create
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Areas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult Create([Bind(Include = "IdArea,NombreArea,DescripcionArea")] Area area)
        {
            if (ModelState.IsValid)
            {

                IEnumerable<int> query = (from c in db.Areas
                                          where c.NombreArea == area.NombreArea
                                          select c.IdArea);
                
                if (query.Count() != 0)
                {
                    ViewBag.areaexistente = "Acción no permitida!. Área existente.";
                    return View(area);

                }


                db.Areas.Add(area);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(area);
        }

        // GET: Areas/Edit/5
        [Authorize(Roles = "Editar Area/Organizacion,Administrador")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Areas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editar Area/Organizacion,Administrador")]
        public ActionResult Edit([Bind(Include = "IdArea,NombreArea,DescripcionArea")] Area area)
        {
            if (ModelState.IsValid)
            {
                db.Entry(area).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(area);
        }

        // GET: Areas/Delete/5
        [Authorize(Roles = "Eliminar Area/Organizacion,Administrador")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Eliminar Area/Organizacion,Administrador")]
        public ActionResult DeleteConfirmed(int id)
        {
            Area area = db.Areas.Find(id);
            
            if (area.PSs.Count() !=0)
            {
                ViewBag.errorareaps = "Acción no permitida! Área con PSs relacionadas.";
                return View(area);
            }

            db.Areas.Remove(area);
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
