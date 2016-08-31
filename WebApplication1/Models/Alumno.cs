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

        public string NombreAlu { get; set; }

        public string ApellidoAlu { get; set; }

        [DataType(DataType.EmailAddress)]
        public string CorreoAlu { get; set; }

        public virtual ICollection<PS> PSs { get; set; }
    }
}