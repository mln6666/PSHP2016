using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Area
    {
        [Key]
        public int IdArea { get; set; }

        [Display(Name = "Área")]
        public string NombreArea { get; set; }

        [Display(Name = "Descripción")]
        public string DescripcionArea { get; set; }

        public virtual ICollection<PS> PSs { get; set; }
    }
}