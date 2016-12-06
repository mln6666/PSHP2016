using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class PresentacionPlanesController : Controller
	{
		private ContextPS db = new ContextPS();

		// GET: PresentacionPlanes
		[Authorize(Roles = "Moderador,Invitado,Administrador")]
		public ActionResult Index()
		{
			var presentacionPlans = db.PresentacionPlanes.Include(p => p.PS);
			return View(presentacionPlans.ToList());
		}

        [Authorize(Roles = "Moderador,Administrador")]
        public JsonResult ArchivoPlanExistente(string nombreArchivo)
        {

            var existe = db.PresentacionPlanes.ToList().Exists(a => a.Archivo == nombreArchivo);
            

            return Json(existe, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "Moderador,Invitado,Administrador")]
		public ActionResult HistorialPlanes(int id)
		{
			var historial = db.PSs.Include(m => m.PresentacionesPlanes).SingleOrDefault(m => m.IdPS == id);
			IEnumerable<PresentacionPlan> prueba = historial.PresentacionesPlanes.OrderByDescending(c => c.IdPresentacionPlan);
			historial.PresentacionesPlanes = prueba.ToList();
			return View(historial);
		}


		// GET: PresentacionPlanes/Details/5
		[Authorize(Roles = "Moderador,Invitado,Administrador")]
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
		[Authorize(Roles = "Moderador,Invitado,Administrador")]
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
		[Authorize(Roles = "Moderador,Administrador")]
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
		[Authorize(Roles = "Moderador,Administrador")]
		public ActionResult Create([Bind(Include = "IdPresentacionPlan,FechaPresentacionPlan,FechaEvaluacionPlan,EstadoEvaluacionPlan,ObservacionesPlan,IdPS")] PresentacionPlan presentacionPlan, HttpPostedFileBase uploadFile, int idPS)
		{
			if (ModelState.IsValid)
			{
				
				if (uploadFile != null && uploadFile.ContentLength > 0) { 
					
					try
					{
						
						string path = Path.Combine(Server.MapPath("~/Content/Files/Planes/"),
												   Path.GetFileName(uploadFile.FileName));
						uploadFile.SaveAs(path);
						presentacionPlan.Archivo = Path.GetFileName(uploadFile.FileName);
						
						
					}
					catch (Exception ex)
					{
						ViewBag.Message = "ERROR:" + ex.Message.ToString();
					}
				}
				

				db.PresentacionPlanes.Add(presentacionPlan);

			   
				db.SaveChanges();

				PS pS = db.PSs.Find(presentacionPlan.IdPS);
				if (presentacionPlan.EstadoEvaluacionPlan==Evaluacion.Pendiente)
					pS.Estado=Estado.Plan_Entregado;
				
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
			int id = presentacionPlan.IdPS;
			PresentacionPlan presentacionplan = new PresentacionPlan();
			presentacionplan.IdPS = id;
			return View(presentacionplan);
		}

		public ActionResult Download(int id)
		{
			//var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/Files/Planes/"));
			//System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");
			//List<string> items = new List<string>();

			//foreach (var file in fileNames)
			//{

			//    items.Add(file.Name);
			//}

			
			string archivo =
				db.PresentacionPlanes.ToList().Find(p => p.IdPresentacionPlan == id).Archivo;
			string ext = archivo.Split('.')[1];

			

			
			var file = File("~/Content/Files/Planes/" + archivo, System.Net.Mime.MediaTypeNames.Application.Pdf);

			

			return (file);

		}

		// GET: PresentacionPlanes/Edit/5
		[Authorize(Roles = "Editar Plan/Informe,Administrador")]
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
			PS ps = db.PSs.ToList().Find(p => p.IdPS == presentacionPlan.IdPS);
			ViewBag.ciclolectivo = ps.CicloLectivo;
			ViewBag.IdPS = new SelectList(db.PSs, "IdPS", "Tutor", presentacionPlan.IdPS);
			return View(presentacionPlan);
		}

		// POST: PresentacionPlanes/Edit/5
		// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
		// más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[Authorize(Roles = "Editar Plan/Informe,Administrador")]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "IdPresentacionPlan,FechaPresentacionPlan,FechaEvaluacionPlan,EstadoEvaluacionPlan,ObservacionesPlan,IdPS")] PresentacionPlan presentacionPlan)
		{
			if (ModelState.IsValid)
			{
				PS pS = db.PSs.Find(presentacionPlan.IdPS);

				
				db.Entry(presentacionPlan).State = EntityState.Modified;
				db.SaveChanges();

				if ((presentacionPlan.IdPresentacionPlan == pS.PresentacionesPlanes.FirstOrDefault().IdPresentacionPlan))
				{
					pS.Vencimiento = presentacionPlan.FechaPresentacionPlan.AddYears(1);
				}

				if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Pendiente)
					pS.Estado = Estado.Plan_Entregado;

				if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Aprobado) { 
					pS.Estado = Estado.Plan_Aprobado;
					pS.Vencimiento = presentacionPlan.FechaEvaluacionPlan.Value.AddYears(1);
				}

				if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Desaprobado)
					pS.Estado = Estado.Plan_Desaprobado;
				if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Rechazado)
				{
					pS.Estado = Estado.Plan_Rechazado;
					pS.FechaFinalizacion = presentacionPlan.FechaEvaluacionPlan;

				}

				db.Entry(pS).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Details", "PS", new { id = presentacionPlan.IdPS });
			}
			ViewBag.IdPS = new SelectList(db.PSs, "IdPS", "Tutor", presentacionPlan.IdPS);
			return View(presentacionPlan);
		}

		// GET: PresentacionPlanes/Edit/5
		[Authorize(Roles = "Moderador,Administrador")]
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
		[Authorize(Roles = "Moderador,Administrador")]
		public ActionResult EvaluarPlan([Bind(Include = "IdPresentacionPlan,FechaPresentacionPlan,FechaEvaluacionPlan,EstadoEvaluacionPlan,ObservacionesPlan,IdPS")] PresentacionPlan presentacionPlan)
		{

			if (ModelState.IsValid)
			{
				PS pS = db.PSs.Find(presentacionPlan.IdPS);

				

				db.Entry(presentacionPlan).State = EntityState.Modified;
				db.SaveChanges();

				if ((presentacionPlan.IdPresentacionPlan == pS.PresentacionesPlanes.LastOrDefault().IdPresentacionPlan) &
				  presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Aprobado)
				{
					pS.Vencimiento = presentacionPlan.FechaPresentacionPlan.AddYears(1);
				}

				if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Pendiente)
					pS.Estado = Estado.Plan_Entregado;

				if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Aprobado) { 
					pS.Estado = Estado.Plan_Aprobado;
					pS.Vencimiento = DateTime.Now.AddYears(1);
				}
				if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Desaprobado)
					pS.Estado = Estado.Plan_Desaprobado;

				if (presentacionPlan.EstadoEvaluacionPlan == Evaluacion.Rechazado)
				{
					pS.Estado = Estado.Plan_Rechazado;
					pS.FechaFinalizacion = presentacionPlan.FechaEvaluacionPlan;
				}


				db.Entry(pS).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Details", "PS", new { id = presentacionPlan.IdPS });
			}

			return PartialView(presentacionPlan);
		}

		//GET
		[Authorize(Roles = "Moderador,Administrador")]
		public ActionResult EliminarArchivo(int idPresentacionPlan)
		{
			PresentacionPlan plan = db.PresentacionPlanes.Find(idPresentacionPlan);
			return View(plan);
		}

		[HttpPost]
		[Authorize(Roles = "Moderador,Administrador")]
		public ActionResult EliminarArchivo([Bind(Include = "IdPresentacionPlan,FechaPresentacionPlan,FechaEvaluacionPlan,EstadoEvaluacionPlan,ObservacionesPlan,IdPS")]PresentacionPlan planEnt)
		{
			PresentacionPlan plan = db.PresentacionPlanes.Find(planEnt.IdPresentacionPlan);
			string path = Path.Combine(Server.MapPath("~/Content/Files/Planes/"),
											   Path.GetFileName(plan.Archivo));

			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}
			

			plan.Archivo = null;

			db.Entry(plan).State = EntityState.Modified;
			db.SaveChanges();


			return RedirectToAction("Details", "PS", new { id = plan.IdPS });
		}


		// GET
		[Authorize(Roles = "Moderador,Administrador")]
		public ActionResult Upload(int idPresentacionPlan)
		{
			PresentacionPlan plan = db.PresentacionPlanes.Find(idPresentacionPlan);
			return View(plan);
		}

		[HttpPost]
		public ActionResult Upload([Bind(Include = "IdPresentacionPlan,FechaPresentacionPlan,FechaEvaluacionPlan,EstadoEvaluacionPlan,ObservacionesPlan,IdPS" )]PresentacionPlan plan, HttpPostedFileBase uploadFile)
		{

			PresentacionPlan presentacionPlan = db.PresentacionPlanes.Find(plan.IdPresentacionPlan);

			if (uploadFile != null && uploadFile.ContentLength > 0)
			{
				//bloque para tratar el caso en que los archivos se llamen igual    
				//foreach (var itemPlan in db.PresentacionPlanes)
				//{
				//	if (uploadFile.FileName == itemPlan.Archivo)
				//	{

				//	}
				//}

				

				try
				{

					string path = Path.Combine(Server.MapPath("~/Content/Files/Planes/"),
											   Path.GetFileName(uploadFile.FileName));
					uploadFile.SaveAs(path);
					presentacionPlan.Archivo = Path.GetFileName(uploadFile.FileName);
					db.Entry(presentacionPlan).State = EntityState.Modified;
					db.SaveChanges();

				}
				catch (Exception ex)
				{
					ViewBag.Message = "ERROR:" + ex.Message.ToString();
				}
			}
			return RedirectToAction("Details", "PS", new { id = plan.IdPS });
		}


		// GET: PresentacionPlanes/Delete/5
		[Authorize(Roles = "Administrador")]
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
		[Authorize(Roles = "Administrador")]
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
