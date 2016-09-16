using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApplication1.Models
{
    public enum Estado

    {
        [Display(Name = "Plan Pendiente")]
        [EnumMember(Value = "Plan Pendiente")]
        Plan_Pendiente = 0,

        [Display(Name = "Plan Entregado")]
        [EnumMember(Value = "Plan Entregado")]
        Plan_Entregado = 1,

        [Display(Name = "Plan Desaprobado")]
        [EnumMember(Value = "Plan Desaprobado")]
        Plan_Desaprobado =2,

        [Display(Name = "Plan Rechazado")]
        [EnumMember(Value = "Plan Rechazado")]
        Plan_Rechazado =3,

        [Display(Name = "Plan Aprobado")]
        [EnumMember(Value = "Plan Aprobado")]
        Plan_Aprobado =4,

        [Display(Name = "Informe Entregado")]
        [EnumMember(Value = "Informe Entregado")]
        Informe_Entregado =5,

        [Display(Name = "Informe Desaprobado")]
        [EnumMember(Value = "Informe Desaprobado")]
        Informe_Desaprobado =6,

        [Display(Name = "PS Vencida")]
        [EnumMember(Value = "PS Vencida")]
        PS_Vencida =7,

        [Display(Name = "Informe Aprobado")]
        [EnumMember(Value = "Informe Aprobado")]
        Informe_Aprobado =8,

        [Display(Name = "PS Aprobada")]
        [EnumMember(Value = "PS Aprobada")]
        PS_Aprobada =9,

        [Display(Name = "PS Cancelada")]
        [EnumMember(Value = "PS Cancelada")]
        PS_Cancelada =10
    }
}