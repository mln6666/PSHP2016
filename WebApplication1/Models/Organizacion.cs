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

        public string DenominacionOrg { get; set; }

        public string DireccionOrg { get; set; }

        public int TelefonoOrg { get; set; }

        public string DescripcionOrg { get; set; }

        public virtual ICollection<PS> PSs { get; set; }
    }
}