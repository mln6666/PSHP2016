using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Context;

namespace WebApplication1.Models
{
    public class LegajoAlumnoExistente : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var alumno = (Alumno)validationContext.ObjectInstance;
            ContextPS db = new ContextPS();
            

            IEnumerable<int> query = (from c in db.Alumnos
                                      where c.Legajo == alumno.Legajo
                                      select c.IdAlumno);

            if (query.Count() != 0)
            {
                return new ValidationResult("Legajo existente.");
            }

            return ValidationResult.Success;
             



        }


    }
}