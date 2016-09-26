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
    [Authorize(Roles = "Administrador")]
    public class AlumnosController : Controller
    {
        private ContextPS db = new ContextPS();

        // GET: Alumnos
        [Authorize(Roles = "Moderador,Invitado")]
        public ActionResult Index()
        {

            var pss = (   from p in db.PSs
                           where p.Estado == Estado.PS_Aprobada /*| p.Estado == Estado.PS_Cancelada */
                           | p.Estado == Estado.PS_Vencida /*| p.Estado == Estado.Plan_Rechazado*/
                           select p);

            var alumnos = db.Alumnos.ToList();

            foreach (var item in pss)
            {
                alumnos.Remove(item.Alumno);
            }

            //where pss.Contains() == a.PSs.LastOrDefault().IdPS
            //select a;

            //IQueryable<Alumno> alumnos = Enumerable.Empty<Alumno>().AsQueryable();

            //foreach (var item in psList)
            //{
            //      alumnos = from a in db.Alumnos
            //              where item.IdPS == a.PSs.LastOrDefault().IdPS
            //              select a;


            //}

            
            return View(alumnos);
        }

        // GET: Alumnos/Details/5
        [Authorize(Roles = "Moderador,Invitado")]
        public ActionResult Details(int? id, bool var)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnos.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            ViewBag.var = var;
            return View(alumno);
        }

        // GET: Alumnos/Create
        [Authorize(Roles = "Moderador")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Alumnos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdAlumno,Legajo,NombreAlu,ApellidoAlu,CorreoAlu,AñoIngreso,Telefono,Celular,Direccion,DNI")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<int> query = (from c in db.Alumnos
                                          where c.Legajo == alumno.Legajo
                                          select c.IdAlumno);

                if (query.Count() != 0)
                {
                    ViewBag.alumnoexistente = "Acción no permitida!. Legajo existente.";
                    return View(alumno);

                }


                db.Alumnos.Add(alumno);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alumno);
        }

        // GET: Alumnos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnos.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // POST: Alumnos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdAlumno,Legajo,NombreAlu,ApellidoAlu,CorreoAlu,AñoIngreso,Telefono,Celular,Direccion,DNI")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alumno).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(alumno);
        }

        // GET: Alumnos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumnos.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // POST: Alumnos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Alumno alumno = db.Alumnos.Find(id);

            if (alumno.PSs.Count() != 0)
            {
                ViewBag.erroralumnops = "Acción no permitida! Alumno con PSs relacionadas.";
                return View(alumno);
            }
            db.Alumnos.Remove(alumno);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult HistorialAlu()
        {
            var alus = db.Alumnos.ToList();
                       
            return View(alus);
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
