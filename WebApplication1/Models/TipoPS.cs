using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TipoPS
    {
        [Key]
        public int IdTipoPS { get; set; }

        public string NombreTipoPS { get; set; }

        public string DescripcionTipoPS { get; set; }

        public virtual ICollection<PS> PSs { get; set; }
    }
}