using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PresentacionInforme
    {
        [Key]
        public int IdPresentacionInforme { get; set; }
                
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime FechaPresentacionInforme { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FechaEvaluacionInforme { get; set; }
                
        public virtual Evaluacion EstadoEvaluacionInforme { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones")]
        public string ObservacionesInforme { get; set; }

        public int IdPS { get; set; }

        public virtual PS PS { get; set; }
    }
}