using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Context;

namespace WebApplication1.Models.Validaciones
{
    public class AreaExistente : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var area = (Area)validationContext.ObjectInstance;
            ContextPS db = new ContextPS();


            IEnumerable<int> query = (from c in db.Areas
                                      where c.NombreArea == area.NombreArea
                                      select c.IdArea);

            if (query.Count() != 0)
            {
                return new ValidationResult("Área existente.");
            }

            return ValidationResult.Success;




        }


    }
}