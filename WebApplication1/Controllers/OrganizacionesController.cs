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

        public ActionResult _NuevaOrg(int? prueba)
        {
            var lista = db.Organizaciones.ToList();
            Organizacion ultima = lista.LastOrDefault();
            ViewBag.organizaciones = lista;

            if (prueba == null) { 
            ViewBag.idultima = ultima.IdOrganizacion;
            ViewBag.denominacionultima = ultima.DenominacionOrg;
            }
            if (prueba != null)
            {
                ViewBag.organizacionexistente = "La organización ya se encontraba en la BD.";
                Organizacion datosOrganizacion = db.Organizaciones.Find(prueba);
                ViewBag.idultima = datosOrganizacion.IdOrganizacion;
                ViewBag.denominacionultima = datosOrganizacion.DenominacionOrg;
            }
            return View();
        }

       

        // POST: Organizaciones/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create1([Bind(Include = "IdOrganizacion,DenominacionOrg,DireccionOrg,TelefonoOrg,DescripcionOrg")] Organizacion organizacion)
        {
            
            if (ModelState.IsValid)
            {

                IEnumerable<int> query = (from c in db.Organizaciones
                                          where c.DenominacionOrg == organizacion.DenominacionOrg
                                          select c.IdOrganizacion);

                if (query.Count() == 0)
                {
                    db.Organizaciones.Add(organizacion);
                    db.SaveChanges();

                    return RedirectToAction("_NuevaOrg");
                    
                }
                if (query.Count()!=0)
                {
                    int prueba = query.ElementAt(query.Count() - 1);
                   
                    return RedirectToAction("_NuevaOrg", new { prueba=prueba});
                }
               
                
            }

            return RedirectToAction("_NuevaOrg");

        }

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
                IEnumerable<int> query = (from c in db.Organizaciones
                                          where c.DenominacionOrg == organizacion.DenominacionOrg
                                          select c.IdOrganizacion);



                if (query.Count() != 0)
                {
                    ViewBag.organizacionexistente = "Acción no permitida!. Organización existente.";
                    return View(organizacion);

                }


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

            if (organizacion.PSs.Count() != 0)
            {
                ViewBag.errororganizacionps = "Acción no permitida! Organización con PSs relacionadas.";
                return View(organizacion);
            }


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
