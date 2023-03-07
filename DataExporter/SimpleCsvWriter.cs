using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;

namespace DataExporter {
    public class SimpleCsvWriter
    {

        private readonly StringBuilder _csv;
        private readonly string _file;

        public SimpleCsvWriter(string file)
        {
            _file = file;
            _csv = new StringBuilder();
        }

        public void AddRow(IEnumerable<string> values)
        {
            _csv.AppendLine(string.Join(",", values));

        }

        public void Save()
        {
            File.WriteAllText(_file, _csv.ToString());

        }
    }
}
