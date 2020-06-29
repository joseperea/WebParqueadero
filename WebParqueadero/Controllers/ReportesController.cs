using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebParqueadero.Models;
using WebParqueadero.ModelViews;

namespace WebParqueadero.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ReportesController : Controller
    {
        private WebParqueaderoContext db = new WebParqueaderoContext();
        // GET: Resportes

        public Documento GetCalculoHoraValor(Documento documento)
        {
            Documento documento1 = new Documento();
            try
            {
                documento1 = documento;
                if (documento1 == null)
                {
                    throw new Exception("Por favor envie el un documento");
                }

                List<DetalleDocumento> detalleDocumentos = documento.DetalleDocumento.ToList();
                decimal valorTipoVehiculo = documento1.Vehiculo.TipoVehiculo.Valor_TVeh;
                decimal tiempoPagoMinutos = db.Parqueaderoes.Find(documento1.Id_Parq).PagoMinutos_Parq;
                decimal resultado = 0;
                TimeSpan timeSpan = new TimeSpan();

                int diferencia = DateTime.Compare(detalleDocumentos[1].Horas_DDoc, detalleDocumentos[0].Horas_DDoc);
                if (diferencia < 0)
                    timeSpan = detalleDocumentos[0].Horas_DDoc.Subtract(detalleDocumentos[1].Horas_DDoc);
                else
                    timeSpan = detalleDocumentos[1].Horas_DDoc.Subtract(detalleDocumentos[0].Horas_DDoc);
                
                decimal totalMinutos = Convert.ToDecimal(timeSpan.TotalMinutes);

                int m = 0;
                if (timeSpan.Days > 0)
                {
                    m = timeSpan.Days / 30;
                }

                resultado = valorTipoVehiculo / tiempoPagoMinutos;
                resultado = resultado * totalMinutos;

                documento1.Valor_Doc = resultado;

                if (documento1.Parqueadero.Lavar)
                    if (documento1.ValorLavado > 0)
                        resultado = resultado + documento1.ValorLavado;

                if (documento1.Parqueadero.Casillero)
                    if (documento1.ValorCasillero > 0)
                        resultado = resultado + documento1.ValorCasillero;

                documento1.ValorPagado_Doc = resultado;

                if (timeSpan.Days > 0 && timeSpan.Days <= 30)
                    documento1.DetalleDocumento.FirstOrDefault().Transcurrido_DDoc = string.Format("{0} Día(s)", timeSpan.Days);
                else if (timeSpan.Days > 30)
                    documento1.DetalleDocumento.FirstOrDefault().Transcurrido_DDoc = string.Format("{0} Mes(es)", m);
                else
                    documento1.DetalleDocumento.FirstOrDefault().Transcurrido_DDoc = string.Format("{0}:{1}:{2}", detalleDocumentos[0].Horas_DDoc.Subtract(detalleDocumentos[1].Horas_DDoc).Hours.ToString("D2"), detalleDocumentos[0].Horas_DDoc.Subtract(detalleDocumentos[1].Horas_DDoc).Minutes.ToString("D2"), detalleDocumentos[0].Horas_DDoc.Subtract(detalleDocumentos[1].Horas_DDoc).Seconds.ToString("D2"));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return documento1;
        }

        public List<Reportes> reportes(DateTime Desde, DateTime Hasta, Guid id) 
        {
            List<Reportes> ltsReportes = new List<Reportes>();
            List<Documento> ltsDocumentos = new List<Documento>();
            ltsDocumentos = db.Documento.Where(t => t.Id_Parq == id && t.Estado_Doc == false).ToList();
            int i = 0;
            foreach (var item in ltsDocumentos.Where(t => t.FechaCreacion_Doc.Date >= Desde.Date && t.FachaFinalizacion_Doc.Date <= Hasta.Date).ToList())
            {
                Reportes reportes = new Reportes();
                reportes.Id_Doc = item.Id_Doc;
                reportes.Placa = item.Vehiculo.Placa_Veh.ToUpper();
                reportes.TipoVehiculo = item.Vehiculo.TipoVehiculo.Nombre_TVeh;
                reportes.ValorTotal = item.Valor_Doc;
                reportes.ValorTotalModificado = item.ValorPagado_Doc;
                List<DetalleDocumento> detalleDocumentos = item.DetalleDocumento.ToList();
                reportes.HoraIngreso = detalleDocumentos[0].Horas_DDoc.ToShortTimeString();
                reportes.HoraSalida = detalleDocumentos[1].Horas_DDoc.ToShortTimeString();
                reportes.FechaIngreso = item.FechaCreacion_Doc.ToShortDateString();
                reportes.FechaSalida = item.FachaFinalizacion_Doc.ToShortDateString();
                reportes.ModificarValor = item.Parqueadero.ModificarValor_Parq;
                reportes.Contador = i + 1;
                reportes.Id_Parq = id;

                TimeSpan timeSpan = new TimeSpan();

                int diferencia = DateTime.Compare(detalleDocumentos[1].Horas_DDoc, detalleDocumentos[0].Horas_DDoc);
                if (diferencia < 0)
                    timeSpan = detalleDocumentos[0].Horas_DDoc.Subtract(detalleDocumentos[1].Horas_DDoc);
                else
                    timeSpan = detalleDocumentos[1].Horas_DDoc.Subtract(detalleDocumentos[0].Horas_DDoc);
                

                decimal totalMinutos = Convert.ToDecimal(timeSpan.TotalMinutes);
                int meses = 0;
                if (timeSpan.Days > 0)
                {
                    meses = timeSpan.Days / 30;
                }


                string Transcurrido_DDoc = string.Empty;

                if (timeSpan.Days > 0 && timeSpan.Days <= 30)
                    Transcurrido_DDoc = string.Format("{0} Día(s)", timeSpan.Days);
                else if (timeSpan.Days > 30)
                    Transcurrido_DDoc = string.Format("{0} Mes(es)", meses);
                else
                    Transcurrido_DDoc = string.Format("{0}:{1}:{2}", timeSpan.Hours.ToString("D2"), timeSpan.Minutes.ToString("D2"), timeSpan.Seconds.ToString("D2"));

                reportes.TiempoTranscurrido = Transcurrido_DDoc;
                reportes.Casillero = item.Parqueadero.Casillero;
                reportes.Lavar = item.Parqueadero.Lavar;
                reportes.ValorLavado = item.ValorLavado;
                reportes.ValorCasillero = item.ValorCasillero;
                ltsReportes.Add(reportes);
                i++;
            }

            return ltsReportes;
        }
        public ActionResult Index(Guid? id)
        {
            List<Reportes> ltsReportes = new List<Reportes>();
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ltsReportes = reportes(DateTime.Now, DateTime.Now, id.Value);
            }
            catch (Exception)
            {

                throw;
            }
            ViewBag.Id_Parq = id;
            return View(ltsReportes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generar(DateTime Desde, DateTime Hasta, Guid? Id_Parq) 
        {
            try
            {
                Utilidades.Export export = new Utilidades.Export();
                FileStream fs = new FileStream(Server.MapPath(@"~\Archivos\NPOI.xls"), FileMode.Open, FileAccess.Read);
                Parqueadero parqueadero = db.Parqueaderoes.Find(Id_Parq);
                // Getting the complete workbook...
                HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true);
                NPOI.HPSF.DocumentSummaryInformation dsi = NPOI.HPSF.PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = parqueadero.NombreEmpresa_Parq; dsi.Manager = parqueadero.NombreEmpresa_Parq;
                templateWorkbook.DocumentSummaryInformation = dsi;

                // Getting the worksheet by its name...
                HSSFSheet sheet = templateWorkbook.GetSheet("Hoja1") as HSSFSheet;
                DataSet dts = new DataSet();
                DataTable dtb = new DataTable();
                dts = export.ConvertDataSet(reportes(Desde, Hasta, Id_Parq.Value).ToList(), dts);

                if (dts.Tables.Count > 0)
                {
                    dtb = dts.Tables[0];
                }
                int fila = 11, columna = 0, i = 0;
                foreach (DataRow item in dtb.Rows)
                {
                    HSSFRow dataRow = null;
                    IRow row = null;
                    dataRow = sheet.GetRow(fila) as HSSFRow;

                    if (dataRow == null)
                    {
                        row = sheet.CreateRow(fila);
                        dataRow = sheet.GetRow(fila) as HSSFRow;
                        if (dataRow == null)
                            throw new Exception("ha ocurrido un error al crear el archivo .xls");
                    }
                    //ArregloExcel(item);
                    foreach (var item1 in item.ItemArray)
                    {
                        if (i == 0)
                        {
                            if (row != null && row.Count() <= 27)
                                row.CreateCell(columna);

                            if (!string.IsNullOrEmpty(item1.ToString()))
                                dataRow.GetCell(columna).SetCellValue(string.Format(item1.ToString()));
                            else
                                dataRow.GetCell(columna).SetCellValue(string.Format("N/A"));
                        }
                        else
                        {
                            if (row != null && row.Count() <= 27)
                                row.CreateCell(columna);

                            if (!string.IsNullOrEmpty(item1.ToString()))
                                dataRow.GetCell(columna).SetCellValue(item1.ToString());
                            else
                                dataRow.GetCell(columna).SetCellValue(string.Format("N/A"));
                        }
                        // Setting the value 77 at row 5 column 1                                
                        columna++; i++;
                    }
                    fila++; columna = 0; i = 0;
                }
                //GraficaExcel(templateWorkbook, dtb);
                MemoryStream ms = new MemoryStream();
                string fechas = ("(" + Desde.ToString("yyyy-MM-dd") + "-" + Hasta.ToString("yyyy-MM-dd") + ")");
                templateWorkbook.SetSheetName(templateWorkbook.GetSheetIndex(sheet), fechas);
                // Writing the workbook content to the FileStream...
                templateWorkbook.Write(ms);

                // Sending the server processed data back to the user computer...                    
                return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("Reportes" + fechas + ".xls"));
            }
            catch (Exception)
            {

                throw;
            }
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
