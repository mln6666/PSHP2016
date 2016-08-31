using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Alumno
    {
        [Key]
        public int IdAlumno { get; set; }

        public int Legajo { get; set; }

        [Display(Name ="Nombre")]
        public string NombreAlu { get; set; }

        [Display(Name = "Apellido")]
        public string ApellidoAlu { get; set; }

        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        public string CorreoAlu { get; set; }

        public virtual ICollection<PS> PSs { get; set; }
    }
}