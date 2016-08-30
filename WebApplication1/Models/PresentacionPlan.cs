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

        public DateTime FechaPresentacionPlan { get; set; }

        public DateTime FechaEvaluacionPlan { get; set; }

        public string EstadoEvaluacionPlan { get; set; }

        public string ObservacionesPlan { get; set; }

        public int IdPS { get; set; }

        public virtual PS PS { get; set; }
    }
}