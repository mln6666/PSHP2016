using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models.Validaciones;

namespace WebApplication1.Models
{
    public class Organizacion
    {
        [Key]
        public int IdOrganizacion { get; set; }

        [Required]
        [Display(Name = "Organización")]
        public string DenominacionOrg { get; set; }

        [Display(Name = "Dirección")]
        public string DireccionOrg { get; set; }

        
        [Display(Name = "Teléfono")]
        public string TelefonoOrg { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Descripción")]
        public string DescripcionOrg { get; set; }

        public virtual ICollection<PS> PSs { get; set; }
    }
}