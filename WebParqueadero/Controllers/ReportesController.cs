using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebParqueadero.Models;
using WebParqueadero.ModelViews;

namespace WebParqueadero.Controllers
{
    public class ReportesController : Controller
    {
        private WebParqueaderoContext db = new WebParqueaderoContext();
        // GET: Resportes
        public ActionResult Index(Guid? id)
        {
            List<Reportes> ltsReportes = new List<Reportes>();
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                DateTime dateTime = DateTime.Now;
                List<Documento> ltsDocumentos = new List<Documento>();
                ltsDocumentos = db.Documento.Where(t => t.Id_Parq == id).ToList();
                int i = 0;
                foreach (var item in ltsDocumentos.Where(t => t.Estado_Doc == false && t.FechaCreacion_Doc.Date >= dateTime.Date && t.FachaFinalizacion_Doc.Date <= dateTime.Date).ToList())
                {
                    Reportes reportes = new Reportes();
                    reportes.Id_Doc = item.Id_Doc;
                    reportes.Placa = item.Vehiculo.Placa_Veh;
                    reportes.TipoVehiculo = item.Vehiculo.TipoVehiculo.Nombre_TVeh;
                    reportes.ValorTotal = item.Valor_Doc;
                    reportes.ValorTotalModificado = item.ValorPagado_Doc;
                    List<DetalleDocumento> detalleDocumentos = item.DetalleDocumento.ToList();
                    reportes.HoraIngreso = detalleDocumentos[0].Horas_DDoc;
                    reportes.HoraSalida = detalleDocumentos[1].Horas_DDoc;
                    reportes.TiempoTranscurrido = string.Format("{0}:{1}:{2}", reportes.HoraSalida.Subtract(reportes.HoraIngreso).Hours.ToString("D2"), reportes.HoraSalida.Subtract(reportes.HoraIngreso).Minutes.ToString("D2"), reportes.HoraSalida.Subtract(reportes.HoraIngreso).Seconds.ToString("D2")); ;
                    reportes.FechaIngreso = item.FechaCreacion_Doc;
                    reportes.FechaSalida = item.FachaFinalizacion_Doc;
                    reportes.ModificarValor = item.Parqueadero.ModificarValor_Parq;
                    reportes.Contador = i + 1;
                    ltsReportes.Add(reportes);
                    i++;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(ltsReportes);
        }

        // GET: Resportes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Resportes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Resportes/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Resportes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Resportes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Resportes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Resportes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
