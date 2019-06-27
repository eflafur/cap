using GruppoCap.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GruppoCap;

namespace GruppoCap.Core.Mvc
{
    public class ExcelResult : ActionResult
    {
        private const String fileExtension = ".xlsx";
        private readonly String _fileName;
        private readonly DataSet _dataSet;

        // EXCEL RESULT
        private ExcelResult(String fileName) : base()
        {
            if (fileName.IsNullOrWhiteSpace())
                _fileName = "{0}{1}".FormatWith(DateTime.Now.ToISODateTimeString(), fileExtension);
            else
                _fileName = "{0}{1}".FormatWith(fileName.Slugify(100), fileExtension);
        }

        // EXCEL RESULT
        public ExcelResult(DataTable table) : this(table.TableName)
        {
            if (_fileName.IsNullOrWhiteSpace())
                _fileName = "{0}{1}".FormatWith(DateTime.Now.ToISODateTimeString(), fileExtension);

            table.TableName = "ESTRAZIONE";

            _dataSet = new DataSet(table.TableName);
            _dataSet.Tables.Add(table);
        }

        // EXCEL RESULT
        public ExcelResult(DataSet dataSet) : this(dataSet.DataSetName)
        {
            if (_fileName.IsNullOrWhiteSpace())
                _fileName = "{0}{1}".FormatWith(DateTime.Now.ToISODateTimeString(), fileExtension);

            _dataSet = dataSet;
        }

        // EXECUTE RESULT
        public override void ExecuteResult(ControllerContext context)
        {
            MemoryStream stream = XlsxGenerator.GetExcelDocument(_dataSet);
            WriteStream(stream, _fileName);
        }

        // WRITE STREAM
        private static void WriteStream(MemoryStream memoryStream, String excelFileName)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();

            if (excelFileName.IsNullOrWhiteSpace())
                excelFileName = "{0}{1}".FormatWith(DateTime.Now.ToISODateTimeString(), fileExtension);

            context.Response.AddHeader("content-disposition", String.Format("attachment;filename={0}", excelFileName));
            memoryStream.WriteTo(context.Response.OutputStream);
            memoryStream.Close();
            context.Response.End();
        }
    }
}
