using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebParqueadero.Models
{
    public class DetalleDocumento
    {
        [Key]
        public Guid Id_DDoc { get; set; }

        public Guid Id_Doc { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime HoraInicial_DDoc { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime HoraFinalizar_DDoc { get; set; }

        public bool Estado_DDoc { get; set; }



        //Realaciones
        public virtual Documento Documento { get; set; }
    }
}