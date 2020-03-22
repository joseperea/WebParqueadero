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

        public string Placa_Veh { get; set; }

        public bool Estado_veh { get; set; }


        //Realciones
        public virtual TipoVehiculos TipoVehiculo { get; set; }
        public virtual ICollection<Documento> Documento { get; set; }
    }
}