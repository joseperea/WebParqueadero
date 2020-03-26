using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebParqueadero.Models;
using WebParqueadero.ModelViews;

namespace WebParqueadero.Controllers
{
    public class HomeController : Controller
    {
        private WebParqueaderoContext db = new WebParqueaderoContext();
        
        public ActionResult Index()
        {
            List<Parqueadero> ltsParqueadero = new List<Parqueadero>();
            List<TipoVehiculos> ltstiPoVehiculos = new List<TipoVehiculos>();
            List<Documento> ltsDocumentos = new List<Documento>();
            Vehiculo vehiculo = new Vehiculo();
            IngresoVehiculoView ingresoVehiculoView = new IngresoVehiculoView();
            ltsParqueadero = db.Parqueaderoes.ToList();
            ltstiPoVehiculos = db.TipoVehiculos.ToList();
            ltsDocumentos = db.Documento.ToList();
            ingresoVehiculoView.TipoVehiculos = ltstiPoVehiculos;
            ingresoVehiculoView.Vehiculo = vehiculo;
            ingresoVehiculoView.Parqueadero = ltsParqueadero;
            ingresoVehiculoView.Documento = ltsDocumentos.Where(t => t.Estado_Doc == true && t.FechaCreacion_Doc.Date == DateTime.Now.Date).ToList();

            return View(ingresoVehiculoView);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult IngresarVehiculos(IngresoVehiculoView ingresoVehiculoView, string TipoVehiculosView, Guid p)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime dateTime = DateTime.Now;
                    Vehiculo vehiculo = new Vehiculo();
                    Parqueadero parqueadero = new Parqueadero();
                    Documento documento = new Documento();
                    DetalleDocumento detalleDocumento = new DetalleDocumento();
                    vehiculo = db.Vehiculo.Where(t => t.Placa_Veh.ToLower() == ingresoVehiculoView.Vehiculo.Placa_Veh.ToLower()).FirstOrDefault();
                    parqueadero = db.Parqueaderoes.Find(p);

                    if (vehiculo == null)
                    {
                        ingresoVehiculoView.Vehiculo.Id_Veh = Guid.NewGuid();
                        ingresoVehiculoView.Vehiculo.Estado_veh = true;
                        ingresoVehiculoView.Vehiculo.Id_TVeh = Guid.Parse(TipoVehiculosView);
                        vehiculo = ingresoVehiculoView.Vehiculo;
                        db.Vehiculo.Add(vehiculo);
                        db.SaveChanges();
                    }
                    documento.Id_Veh = vehiculo.Id_Veh;
                    documento.Id_Parq = parqueadero.Id_Parq;
                    documento.Id_Doc = Guid.NewGuid();
                    documento.Usuario_Doc = "cjs@hotmail.com";
                    documento.Valor_Doc = 0;
                    documento.Consecutivo = db.Documento.ToList().Count + 1;
                    documento.Estado_Doc = true;
                    documento.FechaCreacion_Doc = dateTime;
                    documento.FachaFinalizacion_Doc = DateTime.MaxValue;
                    db.Documento.Add(documento);
                    db.SaveChanges();

                    detalleDocumento.Estado_DDoc = true;
                    detalleDocumento.Horas_DDoc = dateTime;
                    detalleDocumento.Id_DDoc = Guid.NewGuid();
                    detalleDocumento.Id_Doc = documento.Id_Doc;
                    db.DetalleDocumento.Add(detalleDocumento);
                    db.SaveChanges();

                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Facturar() 
        {
            return View();
        }

        [HttpGet]
        public ActionResult CalculoActomatico() 
        {
            List<Parqueadero> ltsParqueadero = new List<Parqueadero>();
            List<TipoVehiculos> ltstiPoVehiculos = new List<TipoVehiculos>();
            List<Documento> ltsDocumentos = new List<Documento>();
            Vehiculo vehiculo = new Vehiculo();
            IngresoVehiculoView ingresoVehiculoView = new IngresoVehiculoView();
            ltsParqueadero = db.Parqueaderoes.ToList();
            ltstiPoVehiculos = db.TipoVehiculos.ToList();
            ingresoVehiculoView.TipoVehiculos = ltstiPoVehiculos;
            ingresoVehiculoView.Vehiculo = vehiculo;
            ingresoVehiculoView.Parqueadero = ltsParqueadero;
   
            foreach (var item in db.Documento.ToList())
            {

                decimal resultado = db.Parqueaderoes.Find(item.Id_Parq).Valor_Parq / db.Parqueaderoes.Find(item.Id_Parq).PagoMinutos_Parq;
                resultado = resultado * Convert.ToDecimal(DateTime.Now.Subtract(item.DetalleDocumento.FirstOrDefault().Horas_DDoc).TotalMinutes);
                item.Valor_Doc = resultado;
                item.DetalleDocumento.FirstOrDefault().Transcurrido_DDoc = string.Format("{0}:{1}", DateTime.Now.Subtract(item.DetalleDocumento.FirstOrDefault().Horas_DDoc).Hours.ToString("D2"), DateTime.Now.Subtract(item.DetalleDocumento.FirstOrDefault().Horas_DDoc).Minutes.ToString("D2"));

                ltsDocumentos.Add(item);
            }

            ingresoVehiculoView.Documento = ltsDocumentos.Where(t => t.Estado_Doc == true && t.FechaCreacion_Doc.Date == DateTime.Now.Date).ToList();

            return PartialView("_CalcularValor", ingresoVehiculoView);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}