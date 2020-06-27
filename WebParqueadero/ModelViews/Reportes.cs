using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebParqueadero.ModelViews
{
    public class Reportes
    {
        [Key]
        public Guid Id_Doc { get; set; }

        public Guid Id_Parq { get; set; }

        [Display(Name = "#")]
        public int Contador { get; set; }

        [Display(Name = "Placa")]
        public string Placa { get; set; }

        [Display(Name = "Tipo")]
        public string TipoVehiculo { get; set; }

        [Display(Name = "Fecha Ingreso")]
        public string FechaIngreso { get; set; }

        [Display(Name = "Fecha Salida")]
        public string FechaSalida { get; set; }

        [Display(Name = "Hora Ingreso")]
        public string HoraIngreso { get; set; }

        [Display(Name = "Hora Salida")]
        public string HoraSalida { get; set; }


        [Display(Name = "Duración")]
        public string TiempoTranscurrido { get; set; }


        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Valor Total")]
        public decimal ValorTotal { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Valor Total Modificado")]
        public decimal ValorTotalModificado { get; set; }

        public bool Lavar { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Valor Lavado")]
        public decimal ValorLavado { get; set; }

        public bool Casillero { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Valor Casillero")]
        public decimal ValorCasillero { get; set; }

        public bool ModificarValor { get; set; }

    }
}