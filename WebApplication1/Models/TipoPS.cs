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

        [Display(Name = "Tipo PS")]
        public string NombreTipoPS { get; set; }

        [Display(Name = "Descripción")]
        public string DescripcionTipoPS { get; set; }

        public virtual ICollection<PS> PSs { get; set; }
    }
}