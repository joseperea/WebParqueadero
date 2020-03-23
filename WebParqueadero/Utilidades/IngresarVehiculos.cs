using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using WebParqueadero.Models;

namespace WebParqueadero.Utilidades
{
    public class IngresarVehiculos
    {
        public void Ingresar(WebParqueaderoContext db) 
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    string strTipoVehiculos = string.Empty;
                    try
                    {
                        strTipoVehiculos = WebConfigurationManager.AppSettings["TipoVehiculosConf"];
                    }
                    catch (Exception)
                    {
                        strTipoVehiculos = string.Empty;
                    }

                    List<string> ltsVehiculos = new List<string>();

                    if (!string.IsNullOrEmpty(strTipoVehiculos))
                    {
                        foreach (var item in strTipoVehiculos.Split(',').ToList())
                        {
                            if (!string.IsNullOrEmpty(item))
                                ltsVehiculos.Add(item);
                        }
                    }
                    else
                        ltsVehiculos = new List<string>() { "Moto", "Carro", "Bicicletas", "Camiones" };
                    
                    List<TipoVehiculos> ltsTipoVehiculos = new List<TipoVehiculos>();
                    ltsTipoVehiculos = db.TipoVehiculos.ToList();

                    foreach (var item in ltsVehiculos)
                    {
                        TipoVehiculos tipoVehiculos = new TipoVehiculos();
                        if (ltsTipoVehiculos.Where(t => t.Nombre_TVeh.ToLower() == item.ToLower()).ToList().Count <= 0)
                        {
                            tipoVehiculos.Id_TVeh = Guid.NewGuid();
                            tipoVehiculos.Nombre_TVeh = item.ToUpper();
                            tipoVehiculos.Descripcion_TVeh = string.Format("Descripción {0}", item);
                            tipoVehiculos.Estado_TVeh = true;
                            tipoVehiculos.CaracterImagen_TVeh = tipoVehiculos.Nombre_TVeh.Substring(0,1).ToUpper();
                            db.TipoVehiculos.Add(tipoVehiculos);
                            db.SaveChanges();
                        }
                    }

                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}