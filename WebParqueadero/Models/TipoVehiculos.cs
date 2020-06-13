using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebParqueadero.Models
{
    public class TipoVehiculos
    {
        [Key]
        public Guid Id_TVeh { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Por favor ingrese el {0} del tipo de vehiculo")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Maximo {1} y minimo a {2} caracteres")]
        public string Nombre_TVeh { get; set; }

        [Display(Name = "Caracter Imagen")]
        //[Required(ErrorMessage = "Por favor ingrese el {0} del tipo de vehiculo")]
        [StringLength(2, MinimumLength = 1, ErrorMessage = "Maximo {1} y minimo a {2} caracteres")]
        public string CaracterImagen_TVeh { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Por favor ingrese el {0} del tipo de vehiculo")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Maximo {1} y minimo a {2} caracteres")]
        public string Descripcion_TVeh { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Por favor ingrese el {0} del tipo de vehiculo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Valor_TVeh { get; set; }

        [Display(Name = "Lavar?")]
        public bool Lavar_TVeh { get; set; }

        [Display(Name = "Valor Lavado")]
        [Required(ErrorMessage = "Por favor ingrese el {0} del tipo de vehiculo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorLavado_TVeh { get; set; }


        [Display(Name = "Casillero?")]
        public bool Casillero_TVeh { get; set; }

        [Display(Name = "Valor Casillero")]
        [Required(ErrorMessage = "Por favor ingrese el {0} del tipo de vehiculo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorCasillero_TVeh { get; set; }

        [Display(Name = "Estado")]
        public bool Estado_TVeh { get; set; }

        //Relaciones
        public virtual ICollection<Vehiculo> Vehiculos { get; set; }
    }
}