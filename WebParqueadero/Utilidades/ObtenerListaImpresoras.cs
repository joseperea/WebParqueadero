using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Printing;
using System.ComponentModel.DataAnnotations;

namespace WebParqueadero.Utilidades
{
    public class Impresoras
    {
        public List<ModelImpresora> ObtenerImpresoras()
        {
            List<ModelImpresora> ltsImpresoras = new List<ModelImpresora>();
            try
            {
                int i = 0;
                foreach (string strPrinter in PrinterSettings.InstalledPrinters)
                {
                    ModelImpresora impresora = new ModelImpresora();
                    impresora.Id_Imp = i + 1;
                    impresora.Nombre_Imp = strPrinter;
                    ltsImpresoras.Add(impresora);
                    i++;
                }

                ModelImpresora impresora1 = new ModelImpresora();
                impresora1.Id_Imp = 0;
                impresora1.Nombre_Imp = "Seleccionar impresora";
                ltsImpresoras.Add(impresora1);

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error al obtener las impresoras: {0}", ex.Message));
            }
            return ltsImpresoras;
        }
    }

    public class ModelImpresora
    {
        public int Id_Imp { get; set; }

        public string Nombre_Imp { get; set; }

    }
}