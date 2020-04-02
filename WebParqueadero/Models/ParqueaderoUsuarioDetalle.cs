using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebParqueadero.Models
{
    public class ParqueaderoUsuarioDetalle
    {
        [Key]
        public Guid Id_PUD { get; set; }
        public Guid Id_Parq { get; set; }
        public string IdUser_PUD { get; set; }


        //Relaciones
        public virtual Parqueadero Parqueadero { get; set; }
    }
}