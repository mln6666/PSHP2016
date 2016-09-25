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

        [Required]
        public int Legajo { get; set; }

        [Required]
        [Display(Name ="Nombre")]
        public string NombreAlu { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string ApellidoAlu { get; set; }

        [Display(Name = "E-Mail")]
        [DataType(DataType.EmailAddress)]
        public string CorreoAlu { get; set; }

        [Display(Name = "Año de Ingreso")]
        public int? AñoIngreso { get; set; }

        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "DNI")]
        public int? DNI { get; set; }

        public virtual ICollection<PS> PSs { get; set; }
    }
}