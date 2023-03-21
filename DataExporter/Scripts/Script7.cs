using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DataExporter.Scripts
{
    public class Script7
    {
        public class Script7DbData
        {
            public DateTime DateTime { get; set; }
            public decimal Actual { get; set; }
            public decimal? Past50s { get; set; }
            public decimal? Past30s { get; set; }
            public decimal? Past10s { get; set; }
            public decimal? Predicted50sAhead { get; set; }
        }
        public static void Run()
        {
            var connection = new SqlConnection("Server=DESKTOP-U28TOVR;Database=TradingBot;Trusted_Connection=True;");
            const string query = "SELECT a.[DateTime], a.[Actual], ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -55, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -45, a.DateTime) ORDER BY b.DateTime DESC ) as Past50s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -35, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -25, a.DateTime) ORDER BY b.DateTime DESC ) as Past30s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -15, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -5, a.DateTime) ORDER BY b.DateTime DESC ) as Past10s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, 45, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, 55, a.DateTime) ORDER BY b.DateTime DESC ) as Predicted50sAhead FROM [dbo].[MarketStateSnaps] a WHERE '2023-03-01 00:00:00.0'< a.DateTime ORDER BY a.DateTime";
            var values = connection.Query<Script7DbData>(query).ToList();
            var a = new SimpleCsvWriter("ma-ta7.csv");

            Console.WriteLine("GotData");
            Console.WriteLine(values.Count);

            values = values.Where(value =>
                value.Past50s.HasValue && value.Past30s.HasValue && value.Past10s.HasValue &&
                value.Predicted50sAhead.HasValue).ToList();

            Console.WriteLine("Filtered data");
            Console.WriteLine(values.Count);

            //a.AddRow(new[] { "Past50s", "Past30s", "Past10s", "Predicted50sAhead" });
            foreach (var value in values)
            {
                var act = value.Actual;

                var row = new decimal?[4];
                //row[0] = value.Actual.ToString();
                row[0] = (value.Past50s - act);
                row[1] = (value.Past30s - act);
                row[2] = (value.Past10s - act);
                row[3] = (value.Predicted50sAhead - act);
                a.AddRow(row.Select(x => (x + Math.Abs(row.Min().Value)).ToString()).Append(value.DateTime.Minute.ToString()));
            }
            a.Save();
        }
    }
}
