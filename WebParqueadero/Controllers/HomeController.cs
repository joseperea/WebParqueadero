using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebParqueadero.Models;
using WebParqueadero.ModelViews;
using WebParqueadero.Utilidades;
using Microsoft.AspNet.Identity;

namespace WebParqueadero.Controllers
{
    public class HomeController : Controller
    {
        private WebParqueaderoContext db = new WebParqueaderoContext();

        public IngresoVehiculoView CagarVista(IngresoVehiculoView ingresoVehiculoView)
        {
            try
            {
                string IdUsuario = User.Identity.GetUserId();
                Parqueadero Parqueadero = new Parqueadero();
                List<TipoVehiculos> ltstiPoVehiculos = new List<TipoVehiculos>();
                List<Documento> ltsDocumentos = new List<Documento>();
                Vehiculo vehiculo = new Vehiculo();
                ParqueaderoUsuarioDetalle parqueaderoUsuarioDetalle = new ParqueaderoUsuarioDetalle();
                parqueaderoUsuarioDetalle = db.ParqueaderoUsuarioDetalle.Where(t => t.IdUser_PUD == IdUsuario).FirstOrDefault();
                Parqueadero = db.Parqueaderoes.Find(parqueaderoUsuarioDetalle.Id_Parq);
                ltstiPoVehiculos = db.TipoVehiculos.ToList();

                foreach (Documento item in db.Documento.Where(t => t.Id_Parq == Parqueadero.Id_Parq && t.Estado_Doc == true).ToList())
                {
                    Documento documento = GetCalculoHoraValor(item);

                    if (documento.FechaCreacion_Doc.Date == DateTime.Now.Date)
                        documento.VehiculosHoy = true;

                    ltsDocumentos.Add(documento);
                }

                ingresoVehiculoView.TipoVehiculos = ltstiPoVehiculos;
                ingresoVehiculoView.Vehiculo = vehiculo;
                ingresoVehiculoView.Parqueadero = Parqueadero;
                ingresoVehiculoView.Documento = ltsDocumentos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ingresoVehiculoView;
        }

        [Authorize]
        public ActionResult Index()
        {
            IngresoVehiculoView ingresoVehiculoView = new IngresoVehiculoView();
            try
            {
                //string IdUsuario = User.Identity.GetUserId();
                //Parqueadero Parqueadero = new Parqueadero();
                //List<TipoVehiculos> ltstiPoVehiculos = new List<TipoVehiculos>();
                //List<Documento> ltsDocumentos = new List<Documento>();
                //Vehiculo vehiculo = new Vehiculo();
                //ParqueaderoUsuarioDetalle parqueaderoUsuarioDetalle = new ParqueaderoUsuarioDetalle();
                //parqueaderoUsuarioDetalle = db.ParqueaderoUsuarioDetalle.Where(t => t.IdUser_PUD == IdUsuario).FirstOrDefault();
                //Parqueadero = db.Parqueaderoes.Find(parqueaderoUsuarioDetalle.Id_Parq);

                //ltstiPoVehiculos = db.TipoVehiculos.ToList();

                //foreach (Documento item in db.Documento.Where(t => t.Id_Parq == Parqueadero.Id_Parq).ToList())
                //{
                //    Documento documento = GetCalculoHoraValor(item);
                //    ltsDocumentos.Add(documento);
                //}

                //ingresoVehiculoView.TipoVehiculos = ltstiPoVehiculos;
                //ingresoVehiculoView.Vehiculo = vehiculo;
                //ingresoVehiculoView.Parqueadero = Parqueadero;
                //ingresoVehiculoView.Documento = ltsDocumentos.Where(t => t.Estado_Doc == true && t.FechaCreacion_Doc.Date == DateTime.Now.Date).ToList();
                ingresoVehiculoView = CagarVista(ingresoVehiculoView);

                if (string.IsNullOrEmpty(ingresoVehiculoView.Parqueadero.Impresora_Parq))
                {
                    return RedirectToAction("Edit", "Parqueadero", new { id = ingresoVehiculoView.Parqueadero.Id_Parq });
                }
            }
            catch (Exception)
            {

                throw;
            }


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

        [Authorize]
        [HttpPost]
        public ActionResult IngresarVehiculos(IngresoVehiculoView ingresoVehiculoView, Guid? TipoVehiculosView, Guid? p)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                try
                {
                    string IdUsuario = User.Identity.GetUserId();
                    if (p == null || p == Guid.Empty)
                        throw new Exception("No tiene ningun parqueadero asignado.");


                    if (TipoVehiculosView == null || TipoVehiculosView == Guid.Empty)
                        throw new Exception("Por favor seleccionar un tipo de vehiculo.");



                    DateTime dateTime = DateTime.Now;
                    Vehiculo vehiculo = new Vehiculo();
                    Parqueadero parqueadero = new Parqueadero();
                    Documento documento = new Documento();
                    DetalleDocumento detalleDocumento = new DetalleDocumento();
                    vehiculo = db.Vehiculo.Where(t => t.Placa_Veh.ToLower() == ingresoVehiculoView.Vehiculo.Placa_Veh.ToLower()).FirstOrDefault();
                    parqueadero = db.Parqueaderoes.Find(p);

                    if (vehiculo == null)
                    {
                        TipoVehiculos tipoVehiculos = new TipoVehiculos();
                        tipoVehiculos = db.TipoVehiculos.Find(TipoVehiculosView);
                        if (tipoVehiculos == null || tipoVehiculos.Id_TVeh == Guid.Empty)
                            throw new Exception("El tipo de vehiculo seleccionado no existe.");


                        ingresoVehiculoView.Vehiculo.Id_Veh = Guid.NewGuid();
                        ingresoVehiculoView.Vehiculo.Estado_veh = true;
                        ingresoVehiculoView.Vehiculo.Id_TVeh = tipoVehiculos.Id_TVeh;
                        vehiculo = ingresoVehiculoView.Vehiculo;
                        db.Vehiculo.Add(vehiculo);
                        db.SaveChanges();
                    }
                    documento.Id_Veh = vehiculo.Id_Veh;
                    documento.Id_Parq = parqueadero.Id_Parq;
                    documento.Id_Doc = Guid.NewGuid();
                    documento.Usuario_Doc = IdUsuario;
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

                    ImprimirTicket imprimirTicket = new ImprimirTicket();
                    Imprimir imprimir = new Imprimir();
                    imprimir.NombreParqueadero = documento.Parqueadero.NombreEmpresa_Parq.ToUpper();
                    imprimir.Direccion = documento.Parqueadero.Direccion_Parq.ToUpper();
                    imprimir.NitParqueadero = documento.Parqueadero.NitEmpresa_Parq.ToUpper();
                    imprimir.TipoVehiculo = db.TipoVehiculos.Find(vehiculo.Id_TVeh).Nombre_TVeh.ToUpper();
                    imprimir.Placa = vehiculo.Placa_Veh.ToUpper();
                    imprimir.Fecha = DateTime.Now.Date;
                    imprimir.Horas = documento.DetalleDocumento.FirstOrDefault().Horas_DDoc;
                    imprimir.Impresora = documento.Parqueadero.Impresora_Parq;
                    imprimir.Transcurrido = GetCalculoHoraValor(documento).DetalleDocumento.FirstOrDefault().Transcurrido_DDoc;
                    imprimirTicket.Generar(imprimir, documento.Parqueadero.ImprimirIngreso_Parq, false);


                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    ModelState.AddModelError(string.Empty, string.Format("Error al ingresar vehiculo: {0}", ex.Message));
                    return View("Index", CagarVista(ingresoVehiculoView));
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Facturar(Documento documento)
        {
            using (var transaccion = db.Database.BeginTransaction())
            {
                IngresoVehiculoView ingresoVehiculoView = new IngresoVehiculoView();
                try
                {
                    if (documento.ValorPagado_Doc <= 0)
                        throw new Exception("Por favor ingrese un valor total valido.");

                    DateTime dateTime = DateTime.Now;
                    Documento documento1 = db.Documento.Find(documento.Id_Doc);


                    if (documento1 == null || documento1.Id_Doc == Guid.Empty)
                        throw new Exception("por favor envié un documento valido.");

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

                    ImprimirTicket imprimirTicket = new ImprimirTicket();
                    Imprimir imprimir = new Imprimir();
                    imprimir.NombreParqueadero = documento1.Parqueadero.NombreEmpresa_Parq.ToUpper();
                    imprimir.Direccion = documento1.Parqueadero.Direccion_Parq.ToUpper();
                    imprimir.NitParqueadero = documento1.Parqueadero.NitEmpresa_Parq.ToUpper();
                    imprimir.TipoVehiculo = db.TipoVehiculos.Find(documento1.Vehiculo.TipoVehiculo.Id_TVeh).Nombre_TVeh.ToUpper();
                    imprimir.Placa = documento1.Vehiculo.Placa_Veh.ToUpper();
                    imprimir.Fecha = DateTime.Now.Date;
                    imprimir.Horas = documento1.DetalleDocumento.FirstOrDefault().Horas_DDoc;
                    imprimir.Impresora = documento1.Parqueadero.Impresora_Parq;
                    imprimir.ValotTotal = documento1.ValorPagado_Doc;
                    imprimir.Transcurrido = GetCalculoHoraValor(documento1).DetalleDocumento.FirstOrDefault().Transcurrido_DDoc;
                    imprimirTicket.Generar(imprimir, documento1.Parqueadero.ImprimirFactura_Parq, true);


                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    ModelState.AddModelError(string.Empty, string.Format("Error al facturar: {0}", ex.Message));
                    return View("Index", CagarVista(ingresoVehiculoView));
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult CalculoActomatico(Guid Id_Doc)
        {
            Documento documento = new Documento();
            IngresoVehiculoView ingresoVehiculoView = new IngresoVehiculoView();
            try
            {
                List<Documento> ltsDocumentos = db.Documento.ToList();
                documento = ltsDocumentos.Where(t => t.Id_Doc == Id_Doc && t.FechaCreacion_Doc.Date <= DateTime.Now.Date).ToList().FirstOrDefault();
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
                ModelState.AddModelError(string.Empty, string.Format("Error al realizar calculo automatico: {0}", ex.Message));
                return View("Index", CagarVista(ingresoVehiculoView));
            }


            return PartialView("_CalcularValorViewPartial", documento);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ViewModalFacturar(Guid Id_Doc)
        {
            Documento documento = new Documento();
            IngresoVehiculoView ingresoVehiculoView = new IngresoVehiculoView();
            try
            {
                List<Documento> ltsDocumentos = db.Documento.ToList();
                documento = ltsDocumentos.Where(t => t.Id_Doc == Id_Doc && t.FechaCreacion_Doc.Date <= DateTime.Now.Date).FirstOrDefault();
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
                ModelState.AddModelError(string.Empty, string.Format("Error al visualizar el modal de factura: {0}", ex.Message));
                return View("Index", CagarVista(ingresoVehiculoView));
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

                decimal valorTipoVehiculo = documento.Vehiculo.TipoVehiculo.Valor_TVeh;
                decimal tiempoPagoMinutos = db.Parqueaderoes.Find(documento.Id_Parq).PagoMinutos_Parq;
                decimal resultado = 0;
                TimeSpan timeSpan = new TimeSpan();
                timeSpan = DateTime.Now.Subtract(documento.DetalleDocumento.FirstOrDefault().Horas_DDoc);
                decimal totalMinutos = Convert.ToDecimal(timeSpan.TotalMinutes);
                int m = timeSpan.Days / 30;

                resultado = valorTipoVehiculo / tiempoPagoMinutos;
                resultado = resultado * totalMinutos;

                documento.Valor_Doc = resultado;
                documento.ValorPagado_Doc = resultado;

                if (timeSpan.Days > 0 && timeSpan.Days <= 30)
                    documento.DetalleDocumento.FirstOrDefault().Transcurrido_DDoc = string.Format("{0} Día(s)", timeSpan.Days);
                else if (timeSpan.Days > 30)
                    documento.DetalleDocumento.FirstOrDefault().Transcurrido_DDoc = string.Format("{0} Mes(es)", m);
                else
                    documento.DetalleDocumento.FirstOrDefault().Transcurrido_DDoc = string.Format("{0}:{1}:{2}", DateTime.Now.Subtract(documento.DetalleDocumento.FirstOrDefault().Horas_DDoc).Hours.ToString("D2"), DateTime.Now.Subtract(documento.DetalleDocumento.FirstOrDefault().Horas_DDoc).Minutes.ToString("D2"), DateTime.Now.Subtract(documento.DetalleDocumento.FirstOrDefault().Horas_DDoc).Seconds.ToString("D2"));

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