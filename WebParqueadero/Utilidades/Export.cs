using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WebParqueadero.ModelViews;

namespace WebParqueadero.Utilidades
{
    public class Export
    {       

        public DataSet ConvertDataSet(List<Reportes> listReports, DataSet dts)
        {
            Reportes reports = new Reportes();
            dts.Tables.Add();
            string TituloExcel = "#,Tipo,Placa,Fecha Ingreso,Fecha Salida,Hora Ingreso,Hora Salida,Duración,Valor Total,Valor Total Modificado";
            foreach (var item in TituloExcel.Split(','))
            {
                dts.Tables[0].Columns.Add(item, typeof(string));
            }

            foreach (Reportes record in listReports)
            {
                dts.Tables[0].Rows.Add(
                    record.Contador,
                    record.TipoVehiculo,
                    record.Placa,
                    record.FechaIngreso,
                    record.FechaSalida,
                    record.HoraIngreso,
                    record.HoraSalida,
                    record.TiempoTranscurrido,
                    record.ValorTotal,
                    record.ValorTotalModificado
                    );
            }
            return dts;
        }
    }
}