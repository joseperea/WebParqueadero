using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebParqueadero.ModelViews
{
    public class FacturarView
    {
        public Guid Id_Fac { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "Por favor ingresar valor transcurrido")]
        public decimal Valor_Fac { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "Por favor ingresar valor")]
        public decimal ValorIngresado_Fac { get; set; }
    }
}