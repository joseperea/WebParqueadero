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
    public class ReportesController : Controller
    {
        private WebParqueaderoContext db = new WebParqueaderoContext();
        // GET: Resportes

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
                reportes.Id_Parq = id;
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
            return View(ltsReportes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generar(DateTime Desde, DateTime Hasta, Guid? Id_Parq) 
        {
            try
            {
                Utilidades.Export export = new Utilidades.Export();
                FileStream fs = new FileStream(Server.MapPath(@"\Archivos\NPOI.xls"), FileMode.Open, FileAccess.Read);
                // Getting the complete workbook...
                HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true);
                NPOI.HPSF.DocumentSummaryInformation dsi = NPOI.HPSF.PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "HOSPITAL SAN JUAN DE DIOS CALI"; dsi.Manager = "SEGURIDAD DEL PACIENTE";
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
                int fila = 12, columna = 0, i = 0;
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
                                dataRow.GetCell(columna).SetCellValue(string.Format("IEA " + item1.ToString()));
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
