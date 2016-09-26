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
        [Authorize(Roles = "Moderador,Invitado,Administrador")]
        public ActionResult Index()
        {
            //if (db.TipoPSs.Count()==0)
            //{
            //    TipoPS tipo1 = new TipoPS();
            //    TipoPS tipo2 = new TipoPS();
            //    TipoPS tipo3 = new TipoPS();
            //    TipoPS tipo4 = new TipoPS();

            //    tipo1.NombreTipoPS = "Pasantía / Laboral";
            //    tipo1.DescripcionTipoPS = "Una Relación Laboral o Pasantía rentada en empresas del medio.";
            //    tipo2.NombreTipoPS = "Proyecto Final";
            //    tipo2.DescripcionTipoPS = "Desarrollo de un Proyecto Final que haya sido acordado mediante un convenio específico con terceros, entes y/o empresas privadas o públicas, grupos de investigación y/o desarrollo tecnológico y de servicios.";
            //    tipo3.NombreTipoPS = "Investigación / Desarrollo";
            //    tipo3.DescripcionTipoPS = "Participación activa en Grupos de Investigación, Desarrollo y/o Aplicación Tecnológica y de Servicios a terceros realizados por Grupos de Investigación o desarrollo tecnológico y de servicios acreditados, perteneciente a instituciones de nivel académico reconocido.";
            //    tipo4.NombreTipoPS = "ONG / Instituciones / Empresas";
            //    tipo4.DescripcionTipoPS = "Desarrollar tareas de ingeniería en el ambito de ONG y/o Instituciones y/o Empresas Productivas o de Servicios públicas o privadas.";
            //    db.TipoPSs.Add(tipo1);
            //    db.TipoPSs.Add(tipo2);
            //    db.TipoPSs.Add(tipo3);
            //    db.TipoPSs.Add(tipo4);
            //    db.SaveChanges();
            //}



            return View(db.TipoPSs.ToList());
        }

        // GET: TipoPSs/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TipoPS tipoPS = db.TipoPSs.Find(id);
        //    if (tipoPS == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tipoPS);
        //}

        // GET: TipoPSs/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: TipoPSs/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "IdTipoPS,NombreTipoPS,DescripcionTipoPS")] TipoPS tipoPS)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.TipoPSs.Add(tipoPS);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(tipoPS);
        //}

        // GET: TipoPSs/Edit/5
        [Authorize(Roles = "Editar TipoPS,Administrador")]
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
        [Authorize(Roles = "Editar TipoPS,Administrador")]
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
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TipoPS tipoPS = db.TipoPSs.Find(id);
        //    if (tipoPS == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tipoPS);
        //}

        // POST: TipoPSs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    TipoPS tipoPS = db.TipoPSs.Find(id);
        //    db.TipoPSs.Remove(tipoPS);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
