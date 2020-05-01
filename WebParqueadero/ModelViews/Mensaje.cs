using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebParqueadero.ModelViews
{
    public class Mensaje
    {
        public string Nombre { get; set; }
        public string DescripcionMensaje { get; set; }

        public bool Error { get; set; }

    }
}