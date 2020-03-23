﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebParqueadero.Models
{
    public class Documento
    {
        [Key]
        public Guid Id_Doc { get; set; }
        public Guid Id_Parq { get; set; }
        public Guid Id_Veh { get; set; }
        public string Usuario_Doc { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCreacion_Doc { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FachaFinalizacion_Doc{ get; set; }
        public decimal Valor_Doc { get; set; }
        public bool Estado_Doc { get; set; }
        public int Consecutivo { get; set; }



        //Realaciones
        public virtual Parqueadero Parqueadero { get; set; }
        public virtual Vehiculo Vehiculo { get; set; }
        public virtual ICollection<DetalleDocumento> DetalleDocumento { get; set; }
    }
}