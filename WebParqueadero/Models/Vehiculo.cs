using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebParqueadero.Models
{
    public class Vehiculo
    {
        [Key]
        public Guid Id_Veh { get; set; }

        public Guid Id_TVeh { get; set; }

        [Required(ErrorMessage = "Por ingrese la placa")]
        [Display(Name = "Placa")]
        [StringLength(8, MinimumLength = 5, ErrorMessage = "Maximo {1} y mayor igual a {2} carateres")]
        public string Placa_Veh { get; set; }

        public bool Estado_veh { get; set; }


        //Realciones
        public virtual TipoVehiculos TipoVehiculo { get; set; }
        public virtual ICollection<Documento> Documento { get; set; }
    }
}