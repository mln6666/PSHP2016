using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpVMsape.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(int error = 0)
        {
            switch (error)
            {
                case 505:
                    ViewBag.Title = "Ocurrio un error inesperado";
                    ViewBag.Description = "Esto es muy vergonzoso, esperemos que no vuelva a pasar ..";
                    break;

                case 404:
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "La URL que está intentando ingresar no existe";
                    break;

                case 2000:
                    ViewBag.Title = "Acción no permitida.";
                    ViewBag.Description = "No se permite evaluar un Plan cuyo estado de evaluación NO es PENDIENTE.";
                    break;
                case 2001:
                    ViewBag.Title = "Acción no permitida.";
                    ViewBag.Description = "No se permite evaluar un Informe cuyo estado de evaluación NO es PENDIENTE.";
                    break;
                case 2002:
                    ViewBag.Title = "Acción no permitida.";
                    ViewBag.Description = "No se permite agregar planes en una PS cuyo estado NO es Plan Pendiente o Plan Desaprobado";
                    break;
                case 2003:
                    ViewBag.Title = "Acción no permitida.";
                    ViewBag.Description = "No se permite agregar planes en una PS cuyo estado NO es Plan Aprobado o Informe Desaprobado";
                    break;
                case 2004:
                    ViewBag.Title = "Acción no permitida.";
                    ViewBag.Description = "No se puede acceder a esta acción si el estado de la PS no es Plan Rechazado.";
                    break;
                case 2005:
                    ViewBag.Title = "Acción no permitida.";
                    ViewBag.Description = "No se permite aprobar una PS cuyo estado NO es Informe Aprobado.";
                    break;
                case 2006:
                    ViewBag.Title = "Acción no permitida.";
                    ViewBag.Description = "No se permite cargar Nro de Disposición a una PS cuyo estado NO es Plan Aprobado.";
                    break;
                case 2007:
                    ViewBag.Title = "Error.";
                    ViewBag.Description = "No se pudo completar la acción. Verifíque que se hayan completado todos los campos correctamente.";
                    break;
                case 2008:
                    ViewBag.Title = "Acción no permitida.";
                    ViewBag.Description = "Solo se puede crear una PS si el alumno no posee PSs o su PS se encuenta Vencida, Cancelada o con Plan Rechazado.";
                    break;
                case 2009:
                    ViewBag.Title = "Acción no permitida.";
                    ViewBag.Description = "Al Cancelar una PS es obligatorio cargar una Observación y la Fecha de Finalización.";
                    break;
                case 2010:
                    ViewBag.Title = "Sin autorización.";
                    ViewBag.Description = "Su usuario no cuenta con los permisos necesarios!";
                    break;
                case 2011:
                    ViewBag.Title = "Acción no permitida.";
                    ViewBag.Description = "No se permite editar una PS que se encuenta Aprobada, Vencida, Cancelada o con Plan Rechazado.";
                    break;
                case 2012:
                    ViewBag.Title = "Acción no permitida.";
                    ViewBag.Description = "Al Vencer una PS es obligatorio cargar una Observación y la Fecha de Finalización.";
                    break;


                default:
                    ViewBag.Title = "Página no encontrada";
                    ViewBag.Description = "Algo salio muy mal :( ..";
                    break;
            }

            return View("~/views/error/_ErrorPage.cshtml");
        }
    }
}