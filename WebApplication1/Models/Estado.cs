using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public enum Estado

    {
        PlanPendiente=0,

        PlanEntregado=1,

        PlanRechazado=2,

        PlanRechazadoDefinitivamente=3,

        PlanAprobado=4,

        InformeEntregado=5,

        InformeRechazado=6,

        PSVencida=7,

        InformeAprobado=8,

        PSAprobada=9,

        PSCancelada=10
    }
}