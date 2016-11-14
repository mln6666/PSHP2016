using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ContadorVenc
    {
        [Key]
        public int IdContadorVenc { get; set; }

        public int CantVenc { get; set; }

        public int CantVencAnt { get; set; }

    }
}