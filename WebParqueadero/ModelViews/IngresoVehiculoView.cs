using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebParqueadero.Models;

namespace WebParqueadero.ModelViews
{
    public class IngresoVehiculoView
    {

        public Vehiculo Vehiculo { get; set; }

        public List<TipoVehiculos> TipoVehiculos { get; set; }

        public Parqueadero Parqueadero { get; set; }

        public List<Documento> Documento { get; set; }

        [StringLength(40,ErrorMessage ="El maximo de caracteres {0} y el minimo {1}",MinimumLength =2)]
        [Display(Name ="Observación casillero")]
        public string Observacion_Casillero { get; set; }

        [Display(Name = "Cantidad de Articulos en el casillero")]
        public int Cantidad_Articulos_Casillero { get; set; }

        [Display(Name = "Lavar?")]
        public bool Lavar { get; set; }

        [Display(Name = "Casillero?")]
        public bool Casillero { get; set; }
    }
}