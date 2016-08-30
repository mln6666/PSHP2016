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

        public DateTime FechaPresentacionInforme { get; set; }

        public DateTime FechaEvaluacionInforme { get; set; }

        public string EstadoEvaluacionInforme { get; set; }

        public string ObservacionesInforme { get; set; }

        public int IdPS { get; set; }

        public virtual PS PS { get; set; }
    }
}