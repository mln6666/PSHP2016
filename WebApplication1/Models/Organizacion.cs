using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Organizacion
    {
        [Key]
        public int IdOrganizacion { get; set; }

        [Display(Name = "Organización")]
        public string DenominacionOrg { get; set; }

        [Display(Name = "Dirección")]
        public string DireccionOrg { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Teléfono")]
        public int TelefonoOrg { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string DescripcionOrg { get; set; }

        public virtual ICollection<PS> PSs { get; set; }
    }
}