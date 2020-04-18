using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebParqueadero.ModelViews
{
    public class Imprimir
    {
        public string NombreParqueadero { get; set; }
        public string NitParqueadero { get; set; }
        public string TipoVehiculo { get; set; }
        public string Placa { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Horas { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValotTotal { get; set; }
        public string Transcurrido { get; set; }
        public string Impresora { get; set; }
        public string Direccion { get; set; }
    }
}