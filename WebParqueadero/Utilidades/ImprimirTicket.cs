using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebParqueadero.ModelViews;

namespace WebParqueadero.Utilidades
{
    public class ImprimirTicket
    {
        public void Generar(Imprimir imprimir, bool GenerarTicket, bool Factura) 
        {
            try
            {
                if (GenerarTicket)
                {
                    CrearTicket crearTicket = new CrearTicket();
                    crearTicket.lineasAsteriscos();
                    crearTicket.TextoCentro("PARQUEADERO");
                    crearTicket.lineasIgual();
                    crearTicket.TextoCentro(imprimir.NombreParqueadero);
                    crearTicket.TextoIzquierda(string.Format("NIT: {0}", imprimir.NitParqueadero));
                    crearTicket.TextoIzquierda(string.Format("DIRECCIÓN: {0}", imprimir.Direccion));
                    crearTicket.lineasAsteriscos();
                    crearTicket.TextoCentro("VEHICULO");
                    crearTicket.lineasIgual();
                    crearTicket.TextoCentro(string.Format("VEHICULO: {0}", imprimir.TipoVehiculo));
                    crearTicket.TextoCentro(string.Format("PLACA: {0}", imprimir.Placa));
                    crearTicket.TextoCentro(string.Format("FECHA: {0}", imprimir.Fecha.ToShortDateString()));
                    crearTicket.TextoCentro(string.Format("HORA: {0}", imprimir.Horas.ToShortTimeString()));

                    if (Factura)
                        crearTicket.TextoCentro(string.Format("DURACIÓN: {0}", imprimir.Transcurrido));

                    crearTicket.lineasAsteriscos();
                    crearTicket.lineasAsteriscos();
                    if (Factura)
                    {
                        crearTicket.TextoExtremos("Total: ", imprimir.ValotTotal.ToString());
                        crearTicket.lineasAsteriscos();
                        crearTicket.lineasAsteriscos();
                    }
                    crearTicket.CortaTicket();
                    crearTicket.ImprimirTicket(imprimir.Impresora);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error al imprimir ticket: {0}", ex.Message));
            }
        }
    }
}