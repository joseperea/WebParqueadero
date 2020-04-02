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
    }
}