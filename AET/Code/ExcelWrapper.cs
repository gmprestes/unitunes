using System;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Linq;

namespace OpenXMLExcel.ExcelUtility
{
    public enum ExcelFormat
    {
        Default = -1,
        Locked = 0,
    }

    public class ExcelStatus
    {
        public string Message { get; set; }
        public bool Success
        {
            get { return string.IsNullOrWhiteSpace(Message); }
        }
    }

    public class ExcelHeader
    {
        public string Header { get; set; }
        public ExcelFormat ColumnFormat { get; set; }

        public ExcelHeader()
        {
            Header = string.Empty;
            ColumnFormat = ExcelFormat.Default;
        }
    }

    public class ExcelData
    {
        public ExcelStatus Status { get; set; }
        //public Columns ColumnConfigurations { get; set; }
        public List<ExcelHeader> Headers { get; set; }
        public List<List<object>> DataRows { get; set; }
        public string SheetName { get; set; }

        public ExcelData()
        {
            Status = new ExcelStatus();
            Headers = new List<ExcelHeader>();
            DataRows = new List<List<object>>();
        }

        public void SetDataRows(string[,] array)
        {
            this.DataRows = new List<List<object>>();
            for (int i = 0, tamanho = array.GetLength(0); i < tamanho; i++)
            {
                this.DataRows.Add(new List<object>());
                for (int j = 0, tamanho2 = array.GetLength(1); j < tamanho2; j++)
                    this.DataRows[i].Add(array[i, j]); ;
            }
        }
    }

    public class ExcelReader
    {
        private string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        private int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            var convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                // ASCII 'A' = 65
                int current = i == 0 ? letter - 65 : letter - 64;
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }

        private IEnumerator<Cell> GetExcelCellEnumerator(Row row)
        {
            int currentCount = 0;
            foreach (Cell cell in row.Descendants<Cell>())
            {
                string columnName = GetColumnName(cell.CellReference);

                int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++)
                {
                    var emptycell = new Cell()
                    {
                        DataType = null,
                        CellValue = new CellValue(string.Empty)
                    };
                    yield return emptycell;
                }

                yield return cell;
                currentCount++;
            }
        }

        private string ReadExcelCell(Cell cell, WorkbookPart workbookPart)
        {
            var cellValue = cell.CellValue;
            var text = (cellValue == null) ? cell.InnerText : cellValue.Text;
            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
            {
                text = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(Convert.ToInt32(cell.CellValue.Text)).InnerText;
            }

            return (text ?? string.Empty).Trim();
        }

        static T First<T>(IEnumerable<T> items)
        {
            using (IEnumerator<T> iter = items.GetEnumerator())
            {
                iter.MoveNext();
                return iter.Current;
            }
        }

        public ExcelData ReadExcel(HttpPostedFileBase file)
        {
            var data = new ExcelData();

            // Check if the file is excel
            if (file.ContentLength <= 0)
            {
                data.Status.Message = "You uploaded an empty file";
                return data;
            }

            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                data.Status.Message = "Please upload a valid excel file of version 2007 and above";
                return data;
            }

            // Open the excel document
            WorkbookPart workbookPart; List<Row> rows;
            try
            {
                var document = SpreadsheetDocument.Open(file.InputStream, false);
                workbookPart = document.WorkbookPart;

                var sheets = workbookPart.Workbook.Descendants<Sheet>();
                var sheet = sheets.First();
                data.SheetName = sheet.Name;

                var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
                var columns = workSheet.Descendants<Columns>().FirstOrDefault();
                //data.ColumnConfigurations = columns;

                var sheetData = workSheet.Elements<SheetData>().First();
                rows = sheetData.Elements<Row>().ToList();
            }
            catch (Exception e)
            {
                data.Status.Message = "Unable to open the file";
                return data;
            }

            // Read the header
            if (rows.Count > 0)
            {
                var row = rows[0];
                var cellEnumerator = GetExcelCellEnumerator(row);
                while (cellEnumerator.MoveNext())
                {
                    var cell = cellEnumerator.Current;
                    var text = ReadExcelCell(cell, workbookPart).Trim();
                    data.Headers.Add(new ExcelHeader() { Header = text });
                }
            }

            // Read the sheet data
            if (rows.Count > 1)
            {
                for (var i = 1; i < rows.Count; i++)
                {
                    var dataRow = new List<object>();
                    data.DataRows.Add(dataRow);
                    var row = rows[i];
                    var cellEnumerator = GetExcelCellEnumerator(row);
                    while (cellEnumerator.MoveNext())
                    {
                        var cell = cellEnumerator.Current;
                        var text = ReadExcelCell(cell, workbookPart).Trim();
                        dataRow.Add(text);
                    }
                }
            }

            return data;
        }
    }

    public class ExcelWriter
    {
        private string ColumnLetter(int intCol)
        {
            var intFirstLetter = ((intCol) / 676) + 64;
            var intSecondLetter = ((intCol % 676) / 26) + 64;
            var intThirdLetter = (intCol % 26) + 65;

            var firstLetter = (intFirstLetter > 64)
                ? (char)intFirstLetter : ' ';
            var secondLetter = (intSecondLetter > 64)
                ? (char)intSecondLetter : ' ';
            var thirdLetter = (char)intThirdLetter;

            return string.Concat(firstLetter, secondLetter,
                thirdLetter).Trim();
        }

        private Cell CreateCell(string header, UInt32 index, object text, ExcelFormat format)
        {
            var cell = new Cell
            {
                //DataType = CellValues.InlineString,
                CellReference = header + index
            };

            if (text.GetType() == typeof(Int32) || text.GetType() == typeof(Double) || text.GetType() == typeof(float) || text.GetType() == typeof(Decimal))
            {
                cell.DataType = CellValues.Number;
                cell.CellValue = new CellValue(text.ToString().Replace(",", "."));

            }


            //else if (text.GetType() == typeof(Boolean))
            //{
            //    cell.DataType = CellValues.Boolean;
            //    cell.CellValue = new CellValue(text.ToString());
            //}
            //else if (text.GetType() == typeof(DateTime))
            //{
            //    cell.DataType = CellValues.Date;
            //    cell.CellValue = new CellValue(text.ToString());

            //}
            else
            {
                cell.DataType = CellValues.InlineString;
                var istring = new InlineString();

                var t = new Text { Text = text.ToString() };
                istring.AppendChild(t);

                cell.AppendChild(istring);

            }

            if ((int)format != -1)
                cell.StyleIndex = Convert.ToUInt32((int)format);

            return cell;
        }

        public byte[] GenerateExcel(ExcelData data)
        {
            var stream = new MemoryStream();
            var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);

            var workbookpart = document.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            var sheetData = new SheetData();

            worksheetPart.Worksheet = new Worksheet(sheetData);

            var sheets = document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            var sheet = new Sheet()
            {
                Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = data.SheetName ?? "Sheet 1"
            };
            sheets.AppendChild(sheet);

            //// cria o "STYLE" de locked padrão
            //CellFormat lockFormat = new CellFormat() { ApplyProtection = true, Protection = new Protection() { Locked = true } };
            //WorkbookStylesPart sp = workbookpart.GetPartsOfType<WorkbookStylesPart>().FirstOrDefault();

            //if (sp == null)
            //    sp = workbookpart.AddNewPart<WorkbookStylesPart>();

            //sp.Stylesheet.CellFormats.AppendChild<CellFormat>(lockFormat);
            //sp.Stylesheet.CellFormats.Count = UInt32Value.FromUInt32((uint)sp.Stylesheet.CellFormats.ChildElements.Count);
            //sp.Stylesheet.Save();


            // Add header
            UInt32 rowIdex = 0;
            var row = new Row { RowIndex = ++rowIdex };
            sheetData.AppendChild(row);
            var cellIdex = 0;

            foreach (var header in data.Headers)
            {
                row.AppendChild(CreateCell(ColumnLetter(cellIdex++), rowIdex, header.Header ?? string.Empty, header.ColumnFormat));
            }

            //if (data.Headers.Count > 0)
            //{
            //    // Add the column configuration if available
            //    if (data.ColumnConfigurations != null)
            //    {
            //        var columns = (Columns)data.ColumnConfigurations.Clone();
            //        worksheetPart.Worksheet.InsertAfter(columns, worksheetPart.Worksheet.SheetFormatProperties);
            //    }
            //}

            // Add sheet data
            foreach (var rowData in data.DataRows)
            {
                cellIdex = 0;
                row = new Row { RowIndex = ++rowIdex };
                sheetData.AppendChild(row);

                var format = ExcelFormat.Default;
                if (data.Headers.Count() >= rowIdex)
                    format = (data.Headers[Convert.ToInt32(rowIdex) - 1]).ColumnFormat;

                foreach (var callData in rowData)
                {
                    var cell = CreateCell(ColumnLetter(cellIdex++), rowIdex, callData ?? string.Empty, format);
                    row.AppendChild(cell);
                }
            }

            workbookpart.Workbook.Save();
            document.Close();

            return stream.ToArray();
        }
    }

}