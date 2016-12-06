using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class PrimerPlanVM
    {

        public int IdPS { get; set; }


        [Display(Name = "Disposición")]
        public string NroDisposicion { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FechaFinalizacion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones Grales")]
        public string ObservacionesPS { get; set; }

        public string Tutor { get; set; }

        [Display(Name = "Título Proyecto")]
        public string TituloProyecto { get; set; }

        [Required]
        [Display(Name = "Ciclo Lectivo")]
        [Range(2000, 2999, ErrorMessage = "Ciclo Lectivo no válido")]
        public int CicloLectivo { get; set; }

        [Required]
        [Display(Name = "Cuatrimestre")]
        public int Cuatrimestre { get; set; }

        public int? IdOrganizacion { get; set; }

        public int? IdArea { get; set; }

        public int? IdTipoPS { get; set; }

        public int IdAlumno { get; set; }

        public virtual Area Area { get; set; }

        public virtual Organizacion Organizacion { get; set; }

        public virtual Estado Estado { get; set; }

        public virtual ICollection<PresentacionPlan> PresentacionesPlanes { get; set; }

        public virtual ICollection<PresentacionInforme> PresentacionesInforme { get; set; }

        public virtual TipoPS TipoPS { get; set; }

        public virtual Alumno Alumno { get; set; }


        public int IdPresentacionPlan { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime FechaPresentacionPlan { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FechaEvaluacionPlan { get; set; }

        public virtual Evaluacion EstadoEvaluacionPlan { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones")]
        public string ObservacionesPlan { get; set; }
       

        public virtual PS PS { get; set; }
    }
}