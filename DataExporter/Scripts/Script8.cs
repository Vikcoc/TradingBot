using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataExporter.Scripts
{
    public class Script8
    {
        public class Script8DbData
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
            var values = connection.Query<Script8DbData>(query).ToList();
            var a = new SimpleCsvWriter("ma-ta8.csv");

            Console.WriteLine("GotData");
            Console.WriteLine(values.Count);

            var arrValues = values.Where(value =>
                value.Past50s.HasValue && value.Past30s.HasValue && value.Past10s.HasValue &&
                value.Predicted50sAhead.HasValue)
                .Select(x => new decimal[] {x.Actual, x.Past50s!.Value, x.Past30s!.Value, x.Past10s!.Value, x.Predicted50sAhead!.Value}).ToList();

            Console.WriteLine("Filtered data");
            Console.WriteLine(values.Count);

            var min = arrValues.Min(x => x.Min());
            var max = arrValues.Max(x => x.Max());

            Console.WriteLine("Min {0}", min);
            Console.WriteLine("Max {0}", max);

            //a.AddRow(new[] { "Past50s", "Past30s", "Past10s", "Predicted50sAhead" });

            var norm = GetMinMaxNorm(min, max);

            foreach (var value in arrValues)
            {
                a.AddRow(value.Select(x => norm(x).ToString(CultureInfo.InvariantCulture)));
            }
            a.Save();
        }

        public static Func<decimal, decimal> GetMinMaxNorm(decimal min, decimal max) => v => (v - min) / (max - min);
    }
}
