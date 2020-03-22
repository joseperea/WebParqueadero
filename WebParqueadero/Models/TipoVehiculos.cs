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

        public string Nombre_TVeh { get; set; }

        public string Descripcion_TVeh { get; set; }

        public bool Estado_TVeh { get; set; }



        //Relaciones
        public virtual ICollection<Vehiculo> Vehiculos { get; set; }
    }
}