using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebParqueadero.ModelViews
{
    public class GenerarReportes
    {
        public Guid Id_Parq { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Desde")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Por favor ingrese la fecha inicial para la busqueda")]
        public DateTime Desde { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Hasta")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Por favor ingrese la fecha final para la busqueda")]
        public DateTime Hasta { get; set; }
    }
}