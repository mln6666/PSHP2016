using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PresentacionPlan
    {
        [Key]
        public int IdPresentacionPlan { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime FechaPresentacionPlan { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime FechaEvaluacionPlan { get; set; }

        public string EstadoEvaluacionPlan { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Observaciones")]
        public string ObservacionesPlan { get; set; }

        public int IdPS { get; set; }

        public virtual PS PS { get; set; }
    }
}