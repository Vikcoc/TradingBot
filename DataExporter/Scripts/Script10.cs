using Dapper;
using System.Data.SqlClient;
using System.Globalization;

namespace DataExporter.Scripts
{
    public class Script10
    {
        public class DbData
        {
            public DateTime DateTime { get; set; }
            public decimal Actual { get; set; }
        }

        public static void Run()
        {
            var connection = new SqlConnection("Server=DESKTOP-U28TOVR;Database=TradingBot;Trusted_Connection=True;");
            const string query = "SELECT a.[DateTime], a.[Actual] FROM [dbo].[MarketStateSnaps] a WHERE '2023-03-01 00:00:00.0'< a.DateTime ORDER BY a.DateTime";
            var values = connection.Query<DbData>(query).ToList();
            var a = new SimpleCsvWriter("ma-ta11.csv");

            Console.WriteLine("GotData");
            Console.WriteLine(values.Count);

            //var arrValues = values.ToList();

            Console.WriteLine("Filtered data");
            Console.WriteLine(values.Count);

            //var min = values.Min(x => x.Actual);
            //var max = values.Max(x => x.Actual);

            //Console.WriteLine("Min {0}", min);
            //Console.WriteLine("Max {0}", max);

            //a.AddRow(new[] { "Past50s", "Past30s", "Past10s", "Predicted50sAhead" });

            //var norm = GetMinMaxNorm(min, max);

            var days = values.GroupBy(x => x.DateTime.Day);

            foreach (var day in days)
            {
                for (var i = 51; i < day.Count() - 20; i++)
                {
                    var cur = day.Skip(i).First().Actual;
                    var fut = day.Skip(i).Take(20).Average(x => x.Actual);
                    var fav = fut > cur + 0.3M ? 1 : fut < cur - 0.3M ? -1 : 0;
                    a.AddRow(new[]{day.Skip(i-51).Take(50).Average(x => x.Actual), cur, fav}
                        .Select(x => x.ToString(CultureInfo.InvariantCulture)));
                }
            }

            
            a.Save();
        }

        public static Func<decimal, decimal> GetMinMaxNorm(decimal min, decimal max) => v => (v - min) / (max - min);
    }
}
