using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebParqueadero.Models;
using WebParqueadero.ModelViews;
using WebParqueadero.Utilidades;

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

            foreach (Documento item in db.Documento.ToList())
            {
                Documento documento = GetCalculoHoraValor(item);
                ltsDocumentos.Add(documento);
            }

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

                    CrearTicket crearTicket = new CrearTicket();
                    crearTicket.lineasAsteriscos();
                    crearTicket.TextoCentro("PARQUEADERO");
                    crearTicket.lineasIgual();
                    crearTicket.TextoCentro(documento.Parqueadero.NombreEmpresa_Parq.ToUpper());
                    crearTicket.TextoCentro(string.Format("NIT: {0}", documento.Parqueadero.NitEmpresa_Parq.ToUpper()));
                    crearTicket.lineasAsteriscos();
                    crearTicket.TextoCentro("VEHICULO");
                    crearTicket.lineasIgual();
                    crearTicket.TextoCentro(string.Format("VEHICULO: {0}", db.TipoVehiculos.Find(vehiculo.Id_TVeh).Nombre_TVeh.ToUpper()));
                    crearTicket.TextoCentro(string.Format("PLACA: {0}", vehiculo.Placa_Veh.ToUpper()));
                    crearTicket.TextoCentro(string.Format("FECHA: {0}", DateTime.Now.ToString("dd/MM/yyyy")));
                    crearTicket.TextoCentro(string.Format("HORA: {0}", documento.DetalleDocumento.FirstOrDefault().Horas_DDoc.ToString("hh:mm:ss")));
                    crearTicket.lineasAsteriscos();
                    crearTicket.lineasAsteriscos();
                    crearTicket.CortaTicket();
                    crearTicket.ImprimirTicket("Microsoft XPS Document Writer");


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
        public ActionResult Facturar(Documento documento)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime dateTime = DateTime.Now;
                    Documento documento1 = db.Documento.Find(documento.Id_Doc);
                    documento1.Valor_Doc = documento.Valor_Doc;
                    documento1.ValorPagado_Doc = documento.ValorPagado_Doc;
                    documento1.FachaFinalizacion_Doc = dateTime;
                    documento1.Estado_Doc = false;
                    db.Entry(documento1).State = EntityState.Modified;

                    foreach (var item in db.DetalleDocumento.Where(t => t.Id_Doc == documento1.Id_Doc).ToList())
                    {
                        DetalleDocumento detalleDocumento1 = item;
                        detalleDocumento1.Estado_DDoc = false;
                        db.Entry(documento1).State = EntityState.Modified;
                    }

                    DetalleDocumento detalleDocumento = new DetalleDocumento();
                    detalleDocumento.Estado_DDoc = false;
                    detalleDocumento.Horas_DDoc = dateTime;
                    detalleDocumento.Id_Doc = documento1.Id_Doc;
                    detalleDocumento.Id_DDoc = Guid.NewGuid();
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

        [HttpGet]
        public ActionResult CalculoActomatico(Guid Id_Doc)
        {
            Documento documento = new Documento();
            try
            {
                List<Documento> ltsDocumentos = db.Documento.ToList();
                documento = ltsDocumentos.Where(t => t.Id_Doc == Id_Doc && t.FechaCreacion_Doc.Date == DateTime.Now.Date).ToList().FirstOrDefault();
                if (documento != null)
                {
                    documento = GetCalculoHoraValor(documento);
                }
                else
                {
                    documento = new Documento();
                    documento.Id_Doc = Guid.NewGuid();
                    documento.Parqueadero = new Parqueadero();
                    documento.DetalleDocumento = new List<DetalleDocumento>();
                    documento.Vehiculo = new Vehiculo();
                    
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           

            return PartialView("_CalcularValorViewPartial", documento);
        }

        [HttpGet]
        public ActionResult ViewModalFacturar(Guid Id_Doc) 
        {
            Documento documento = new Documento();
            try
            {
                List<Documento> ltsDocumentos = db.Documento.ToList();
                documento = ltsDocumentos.Where(t => t.Id_Doc == Id_Doc && t.FechaCreacion_Doc.Date == DateTime.Now.Date).FirstOrDefault();
                if (documento != null)
                {
                    documento = GetCalculoHoraValor(documento);
                }
                else
                {
                    documento = new Documento();
                    documento.Id_Doc = Guid.NewGuid();
                    documento.Parqueadero = new Parqueadero();
                    documento.DetalleDocumento = new List<DetalleDocumento>();
                    documento.Vehiculo = new Vehiculo();

                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return PartialView("_ModalFacturarViewPartial", documento);
        }

        public Documento GetCalculoHoraValor(Documento documento) 
        {
            try
            {
                if (documento == null)
                {
                    throw new Exception("Por favor envie el un documento");
                }

                decimal resultado = db.Parqueaderoes.Find(documento.Id_Parq).Valor_Parq / db.Parqueaderoes.Find(documento.Id_Parq).PagoMinutos_Parq;
                resultado = resultado * Convert.ToDecimal(DateTime.Now.Subtract(documento.DetalleDocumento.FirstOrDefault().Horas_DDoc).TotalMinutes);
                documento.Valor_Doc = resultado;
                documento.ValorPagado_Doc = resultado;
                documento.DetalleDocumento.FirstOrDefault().Transcurrido_DDoc = string.Format("{0}:{1}", DateTime.Now.Subtract(documento.DetalleDocumento.FirstOrDefault().Horas_DDoc).Hours.ToString("D2"), DateTime.Now.Subtract(documento.DetalleDocumento.FirstOrDefault().Horas_DDoc).Minutes.ToString("D2"));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return documento;
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