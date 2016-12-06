using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
        [Authorize(Roles = "Moderador,Invitado,Administrador")]
        public ActionResult Index()
        {
            var presentacionInformes = db.PresentacionInformes.Include(p => p.PS);
            return View(presentacionInformes.ToList());
        }

        [Authorize(Roles = "Moderador,Invitado,Administrador")]
        public ActionResult HistorialInformes(int id)
        {
            var historial = db.PSs.Include(m => m.PresentacionesInforme).SingleOrDefault(m => m.IdPS == id);
            IEnumerable<PresentacionInforme> prueba = historial.PresentacionesInforme.OrderByDescending(c => c.IdPresentacionInforme);
            historial.PresentacionesInforme = prueba.ToList();
            return View(historial);
        }

        // GET: PresentacionInformes/Details/5
        [Authorize(Roles = "Moderador,Invitado,Administrador")]
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

        [Authorize(Roles = "Moderador,Invitado,Administrador")]
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


            int id = query.ElementAt(query.Count() - 1);
            PresentacionInforme datosinforme = db.PresentacionInformes.Find(id);

            return View("Details", datosinforme);
        }



        // GET: PresentacionInformes/Create
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult Create(int id)
        {
            PresentacionInforme presentacioninforme = new PresentacionInforme();
            presentacioninforme.IdPS = id;
            PS ps = db.PSs.Find(id);


            if (ps.Estado != Estado.Informe_Desaprobado & ps.Estado != Estado.Plan_Aprobado)
            {
                return RedirectToAction("Index", "Error", new { error = 2002 });
            }

            var selectList = Enum.GetValues(typeof(Evaluacion))
                       .Cast<Evaluacion>()
                       .Where(e => e != Evaluacion.Rechazado)
                       .Select(e => new SelectListItem
                       {
                           Value = ((int)e).ToString(),
                           Text = e.ToString()
                       });

            ViewBag.SelectList = selectList;
            ViewBag.fechita = ps.PresentacionesPlanes.LastOrDefault().FechaEvaluacionPlan;
            //ViewBag.IdPS = new SelectList(db.PSs, "IdPS", "Tutor");
            return View(presentacioninforme);
        }

        // POST: PresentacionInformes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult Create([Bind(Include = "IdPresentacionInforme,FechaPresentacionInforme,FechaEvaluacionInforme,EstadoEvaluacionInforme,ObservacionesInforme,IdPS")] PresentacionInforme presentacionInforme)
        {
            if (ModelState.IsValid)
            {
                db.PresentacionInformes.Add(presentacionInforme);
                db.SaveChanges();
                PS pS = db.PSs.Find(presentacionInforme.IdPS);
                if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Pendiente)
                    pS.Estado = Estado.Informe_Entregado;

                if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Aprobado)
                    pS.Estado = Estado.Informe_Aprobado;

                if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Desaprobado)
                    pS.Estado = Estado.Informe_Desaprobado;

                db.Entry(pS).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "PS", new { id = presentacionInforme.IdPS });
            }
            int id = presentacionInforme.IdPS;
            PresentacionInforme presentacioninforme = new PresentacionInforme();
            presentacioninforme.IdPS = id;
            return View(presentacioninforme);
        }

        // GET:
        //public ActionResult EditUltimoInf(int? id)
        //{
        //    IEnumerable<int> query = (from c in db.PresentacionInformes
        //                              where c.IdPS == id
        //                              select c.IdPresentacionInforme);

        //    int idinf = query.ElementAt(query.Count() - 1);
        //    PresentacionInforme presentacionInforme = db.PresentacionInformes.Find(idinf);

        //    return View(presentacionInforme);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditUltimoInf([Bind(Include = "IdPresentacionInforme,FechaPresentacionInforme,FechaEvaluacionInforme,EstadoEvaluacionInforme,ObservacionesInforme,IdPS")] PresentacionInforme presentacionInforme)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(presentacionInforme).State = EntityState.Modified;
        //        db.SaveChanges();

        //        PS pS = db.PSs.Find(presentacionInforme.IdPS);
        //        if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Pendiente)
        //            pS.Estado = Estado.Informe_Entregado;

        //        if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Aprobado)
        //            pS.Estado = Estado.Informe_Aprobado;

        //        if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Desaprobado)
        //            pS.Estado = Estado.Informe_Desaprobado;

        //        db.Entry(pS).State = EntityState.Modified;
        //        db.SaveChanges();

        //        return RedirectToAction("Details", "PS", new { id = presentacionInforme.IdPS });
        //    }
        //    return View(presentacionInforme);
        //}

        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult EvaluarInforme(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            PresentacionInforme presentacionInforme = db.PresentacionInformes.Find(id);
            if (presentacionInforme == null)
            {
                return HttpNotFound();
            }
            if (presentacionInforme.EstadoEvaluacionInforme != Evaluacion.Pendiente)
            {
                return RedirectToAction("Index", "Error", new { error = 2001 });
            }

            var selectList = Enum.GetValues(typeof(Evaluacion))
                        .Cast<Evaluacion>()
                        .Where(e => e != Evaluacion.Rechazado)
                        .Select(e => new SelectListItem
                        {
                            Value = ((int)e).ToString(),
                            Text = e.ToString()
                        });

            ViewBag.SelectList = selectList;
            ViewBag.fechapresentacioninforme = presentacionInforme.FechaPresentacionInforme;
            return PartialView(presentacionInforme);
        }

        // POST: PresentacionPlanes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult EvaluarInforme([Bind(Include = "IdPresentacionInforme,FechaPresentacionInforme,FechaEvaluacionInforme,EstadoEvaluacionInforme,ObservacionesInforme,IdPS")] PresentacionInforme presentacionInforme)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presentacionInforme).State = EntityState.Modified;
                db.SaveChanges();

                PS pS = db.PSs.Find(presentacionInforme.IdPS);
                if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Pendiente)
                    pS.Estado = Estado.Informe_Entregado;

                if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Aprobado)
                    pS.Estado = Estado.Informe_Aprobado;

                if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Desaprobado)
                    pS.Estado = Estado.Informe_Desaprobado;

                db.Entry(pS).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "PS", new { id = presentacionInforme.IdPS });
            }

            
            return PartialView(presentacionInforme);
        }




        // GET: PresentacionInformes/Edit/5
        [Authorize(Roles = "Editar Plan/Informe,Administrador")]
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

            var selectList = Enum.GetValues(typeof(Evaluacion))
                       .Cast<Evaluacion>()
                       .Where(e => e != Evaluacion.Rechazado)
                       .Select(e => new SelectListItem
                       {
                           Value = ((int)e).ToString(),
                           Text = e.ToString()
                       });

            ViewBag.SelectList = selectList;
            return View(presentacionInforme);
        }



        // POST: PresentacionInformes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Editar Plan/Informe,Administrador")]
        public ActionResult Edit([Bind(Include = "IdPresentacionInforme,FechaPresentacionInforme,FechaEvaluacionInforme,EstadoEvaluacionInforme,ObservacionesInforme,IdPS")] PresentacionInforme presentacionInforme)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presentacionInforme).State = EntityState.Modified;
                db.SaveChanges();

                PS pS = db.PSs.Find(presentacionInforme.IdPS);
                if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Pendiente)
                pS.Estado = Estado.Informe_Entregado;

                if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Aprobado)
                pS.Estado = Estado.Informe_Aprobado;

                if (presentacionInforme.EstadoEvaluacionInforme == Evaluacion.Desaprobado)
                pS.Estado = Estado.Informe_Desaprobado;

                db.Entry(pS).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Details", "PS", new {id = presentacionInforme.IdPS});
            }
            ViewBag.IdPS = new SelectList(db.PSs, "IdPS", "Tutor", presentacionInforme.IdPS);
            return View(presentacionInforme);
        }

        // GET: PresentacionInformes/Delete/5
        [Authorize(Roles = "Administrador")]
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

        // GET
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult Upload(int idPresentacionInforme)
        {
            PresentacionInforme informe = db.PresentacionInformes.Find(idPresentacionInforme);
            return View(informe);
        }

        [HttpPost]
        public ActionResult Upload([Bind(Include = "IdPresentacionInforme,FechaPresentacionInforme,FechaEvaluacionInforme,EstadoEvaluacionInforme,ObservacionesInforme,IdPS")]PresentacionInforme informe, HttpPostedFileBase uploadFile)
        {

            PresentacionInforme presentacionInforme = db.PresentacionInformes.Find(informe.IdPresentacionInforme);

            if (uploadFile != null && uploadFile.ContentLength > 0)
            {

                try
                {

                    string path = Path.Combine(Server.MapPath("~/Content/Files/Informes/"),
                                               Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(path);
                    presentacionInforme.Archivo = Path.GetFileName(uploadFile.FileName);
                    db.Entry(presentacionInforme).State = EntityState.Modified;
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            return RedirectToAction("Details", "PS", new { id = informe.IdPS });
        }

        public ActionResult Download(int id)
        {
            
            string archivo =
                db.PresentacionInformes.ToList().Find(p => p.IdPresentacionInforme == id).Archivo;
            string ext = archivo.Split('.')[1];




            var file = File("~/Content/Files/Informes/" + archivo, System.Net.Mime.MediaTypeNames.Application.Pdf);



            return (file);

        }
        //GET
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult EliminarArchivo(int idPresentacionInforme)
        {
            PresentacionInforme informe = db.PresentacionInformes.Find(idPresentacionInforme);
            return View(informe);
        }

        [HttpPost]
        [Authorize(Roles = "Moderador,Administrador")]
        public ActionResult EliminarArchivo([Bind(Include = "IdPresentacionInforme,FechaPresentacionInforme,FechaEvaluacionInforme,EstadoEvaluacionInforme,ObservacionesInforme,IdPS")]PresentacionInforme informeEnt)
        {
            PresentacionInforme informe = db.PresentacionInformes.Find(informeEnt.IdPresentacionInforme);
            string path = Path.Combine(Server.MapPath("~/Content/Files/Informes/"),
                                               Path.GetFileName(informe.Archivo));

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }


            informe.Archivo = null;

            db.Entry(informe).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("Details", "PS", new { id = informe.IdPS });
        }
        // POST: PresentacionInformes/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
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
