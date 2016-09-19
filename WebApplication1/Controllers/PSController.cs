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
    public class PSController : Controller
    {
        private ContextPS db = new ContextPS();

        // GET: PS
        public ActionResult Index()
        {
            var pSs = new List<PS>();
            pSs = db.PSs.ToList();
            for (int i = 0; i < pSs.Count(); i++)
            {
                if (pSs[i].Estado == Estado.PS_Aprobada)
                {
                    pSs.RemoveAt(i);
                }
            }                      

            return View(pSs);
        }

        public ActionResult HistorialPS()
        {            
            var pSs = db.PSs.ToList();
            
            return View(pSs);
        }

        // GET: PS/Details/5
        public ActionResult Details(int? id)
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

        // GET: PS/Create
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
        public ActionResult Create([Bind(Include = "IdPS,NroDisposicion,Tutor,TituloProyecto,CicloLectivo,Cuatrimestre,IdOrganizacion,IdArea,IdTipoPS,IdAlumno")] PS pS)
        {
            if (ModelState.IsValid)
            {
                pS.Estado=Estado.Plan_Pendiente;
                db.PSs.Add(pS);
                db.SaveChanges();
                return RedirectToAction("Details", "PS", new { id = pS.IdPS });
            }

            ViewBag.IdAlumno = new SelectList(db.Alumnos, "IdAlumno", "NombreAlu", pS.IdAlumno);
            ViewBag.IdArea = new SelectList(db.Areas, "IdArea", "NombreArea", pS.IdArea);
            ViewBag.IdOrganizacion = new SelectList(db.Organizaciones, "IdOrganizacion", "DenominacionOrg", pS.IdOrganizacion);
            ViewBag.IdTipoPS = new SelectList(db.TipoPSs, "IdTipoPS", "NombreTipoPS", pS.IdTipoPS);

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
            if (ModelState.IsValid)
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
            ViewBag.alumnos = db.Alumnos.ToList();
            return View();
        }

        [HttpPost]
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
