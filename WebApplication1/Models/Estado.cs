using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public enum Estado
    {
        PlanEntregado,

        PlanRechazado,

        PlanRechazadoDefinitivamente,

        PlanAprobado,

        InformeEntregado,

        InformeRechazado,

        PSVencida,

        InformeAprobado,

        PSAprobada,

        PSCancelada
    }
}