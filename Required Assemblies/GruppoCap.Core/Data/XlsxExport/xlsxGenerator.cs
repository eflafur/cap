using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppoCap.Core.Data
{
    public static class XlsxGenerator
    {
        // GET EXCEL DOCUMENT (GENERICs IENUMERABLE<T>)
        public static MemoryStream GetExcelDocument<T>(IEnumerable<T> list) where T : class, IEntity
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(list.ToDataTable<T>());

                return GetExcelDocument(ds);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        // GET EXCEL DOCUMENT
        public static MemoryStream GetExcelDocument(DataSet ds)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    WriteExcelFile(ds, document);
                }

                stream.Position = 0;
                return stream;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // WRITE EXCEL FILE
        private static void WriteExcelFile(DataSet ds, SpreadsheetDocument spreadsheet)
        {
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");
            Stylesheet stylesheet = new Stylesheet();
            workbookStylesPart.Stylesheet = stylesheet;

            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            UInt32 worksheetNumber = 1;
            foreach (DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                String workSheetID = "rId" + worksheetNumber.ToString();
                //String worksheetName = dt.TableName;

                WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet();

                // create sheet data
                newWorksheetPart.Worksheet.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.SheetData());

                // save worksheet
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart);
                newWorksheetPart.Worksheet.Save();

                // create the worksheet to workbook relation
                if (worksheetNumber == 1)
                    spreadsheet.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

                spreadsheet.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>().AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = (UInt32)worksheetNumber,
                    Name = dt.TableName
                });

                worksheetNumber++;
            }

            spreadsheet.WorkbookPart.Workbook.Save();
        }

        // WRITE DATATABLE TO EXCEL WORKSHEET
        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            String cellValue = "";

            Int32 numberOfColumns = dt.Columns.Count;
            Boolean[] IsNumericColumn = new Boolean[numberOfColumns];

            String[] excelColumnNames = new String[numberOfColumns];
            for (Int32 n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            //
            //  Create the Header row in our Excel Worksheet
            //
            UInt32 rowIndex = 1;

            var headerRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
            sheetData.Append(headerRow);

            for (Int32 colInx = 0; colInx < numberOfColumns; colInx++)
            {
                DataColumn col = dt.Columns[colInx];
                AppendTextCell(excelColumnNames[colInx] + "1", col.ColumnName, headerRow);
                IsNumericColumn[colInx] = (col.DataType.FullName == "System.Decimal") || (col.DataType.FullName == "System.Int32");
            }

            //
            //  Now, step through each row of data in our DataTable...
            //
            Double cellNumericValue = 0;
            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;
                var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);

                for (Int32 colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();

                    // Create cell with data
                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        cellNumericValue = 0;
                        if (Double.TryParse(cellValue, out cellNumericValue))
                        {
                            // Ale - fix per correggere il bug relativo al formato dei numeri che venivano automaticamente
                            // castati a stringa con formato locale (attraverso .ToString() senza specificare una culture)
                            cellValue = cellNumericValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                            AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                        }
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                    }
                }
            }
        }

        // APPEND TEXT CELL
        private static void AppendTextCell(String cellReference, String cellStringValue, Row excelRow)
        {
            Cell cell = new Cell() { CellReference = cellReference, DataType = CellValues.String };
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        // APPEND NUMERIC CELL
        private static void AppendNumericCell(String cellReference, String cellStringValue, Row excelRow)
        {
            // Ale - fix necessaria per far riconoscere in maniera inequivocabile un dato numerico ed impostare la cella come tale
            Cell cell = new Cell() { CellReference = cellReference, DataType = CellValues.Number };
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        // GET EXCEL COLUMN NAME
        private static String GetExcelColumnName(Int32 columnIndex)
        {
            if (columnIndex < 26)
                return ((Char)('A' + columnIndex)).ToString();

            char firstChar = (Char)('A' + (columnIndex / 26) - 1);
            char secondChar = (Char)('A' + (columnIndex % 26));

            return "{0}{1}".FormatWith(firstChar, secondChar);
        }
    }
}
