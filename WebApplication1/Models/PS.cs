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
        public int NroDisposicion { get; set; }

        public string Tutor { get; set; }

        [Display(Name = "Título Proyecto")]
        public string TituloProyecto { get; set; }

        [Display(Name = "Ciclo Lectivo")]
        public int CicloLectivo { get; set; }

        public int Cuatrimestre { get; set; }

        public int IdOrganizacion { get; set; }

        public int IdArea { get; set; }

        public int IdTipoPS { get; set; }

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