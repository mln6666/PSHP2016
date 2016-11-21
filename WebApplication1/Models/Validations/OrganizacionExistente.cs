using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Context;

namespace WebApplication1.Models.Validaciones
{
    public class OrganizacionExistente:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var organizacion = (Organizacion)validationContext.ObjectInstance;
            ContextPS db = new ContextPS();


            IEnumerable<int> query = (from c in db.Organizaciones
                                      where c.DenominacionOrg == organizacion.DenominacionOrg
                                      select c.IdOrganizacion);



            if (query.Count() != 0)
            {
                return new ValidationResult("Organización existente.");
            }
            return ValidationResult.Success;


        }


    }
    
    
}