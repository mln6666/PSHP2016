using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PS
    {
        [Key]
        public int IdPS { get; set; }

        
        [Display(Name = "Disposición")]
        public string NroDisposicion { get; set; }

        public string Tutor { get; set; }

        [Display(Name = "Título Proyecto")]
        public string TituloProyecto { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime? Vencimiento { get; set; }

        [Required]
        [Display(Name = "Ciclo Lectivo")]
        [Range(2000, 2999, ErrorMessage = "Ciclo Lectivo no válido")]
        public int CicloLectivo { get; set; }

        [Required]
        [Display(Name = "Cuatrimestre")]
        public int Cuatrimestre { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Fecha Finalización")]
        public DateTime? FechaFinalizacion { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones Generales")]
        public string ObservacionesPS { get; set; }

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
    }
}