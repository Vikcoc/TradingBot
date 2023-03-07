using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DataExporter {
    public class SimpleExcelWriter : IDisposable
    {
        private readonly SpreadsheetDocument _document;
        private readonly WorksheetPart _part;
        private readonly SheetData _sheet;
        public SimpleExcelWriter(string path)
        {
            _document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);
            var workbookPart = _document.AddWorkbookPart();
            _part = _document.WorkbookPart.AddNewPart<WorksheetPart>();
            _document.WorkbookPart.Workbook = new Workbook();
            _part.Worksheet = new Worksheet();
            _sheet = _part.Worksheet.AppendChild(new SheetData());
        }

        public void AddRow(IEnumerable<string> values)
        {
            var row = _sheet.AppendChild(new Row());
            foreach (var value in values)
                row.AppendChild(new Cell { CellValue = new CellValue(value), DataType = CellValues.String });
        }

        public void Save()
        {
            var sheets = _document.WorkbookPart.Workbook.AppendChild(new Sheets());
            sheets.AppendChild(new Sheet() { Id = _document.WorkbookPart.GetIdOfPart(_part), SheetId = 1, Name = "Sheet" });
            _document.WorkbookPart.Workbook.Save();
        }

        public void Dispose()
        {
            _document.Dispose();
        }
    }
}
