using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataExporter.Scripts
{
    public class Script5
    {
        public static void Run()
        {
            var connection = new SqlConnection("Server=DESKTOP-U28TOVR;Database=TradingBot;Trusted_Connection=True;");
            const string query = "SELECT a.[DateTime], a.[BestBid], a.[BestAsk], a.[Actual], ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -55, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -45, a.DateTime) ORDER BY b.DateTime DESC ) as Past50s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -35, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -25, a.DateTime) ORDER BY b.DateTime DESC ) as Past30s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -15, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -5, a.DateTime) ORDER BY b.DateTime DESC ) as Past10s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, 45, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, 55, a.DateTime) ORDER BY b.DateTime DESC ) as Predicted50sAhead FROM [dbo].[MarketStateSnaps] a WHERE '2023-03-18 00:00:00.0'< a.DateTime AND a.DateTime < '2023-03-19 00:00:00.0' ORDER BY a.DateTime";
            var values = connection.Query<dynamic>(query).ToList();
            var a = new SimpleCsvWriter("ma-ta5.csv");

            Console.WriteLine("GotData");
            Console.WriteLine(values.Count);

            values = values.Where(value =>
                value.Past50s != null && value.Past30s != null && value.Past10s != null &&
                value.Predicted50sAhead != null).ToList();

            Console.WriteLine("Filtered data");
            Console.WriteLine(values.Count);

            a.AddRow(new[] { "CurrentValue", "BestBid", "BestAsk", "Hour", "Minute", "Second", "Millisecond", "Past50s", "Past30s", "Past10s", "Predicted50sAhead" });
            foreach (var value in values)
            {
                var date = (DateTime)value.DateTime;
                var row = new string[11];
                row[0] = value.Actual.ToString();
                row[1] = value.BestBid.ToString();
                row[2] = value.BestAsk.ToString();
                row[3] = date.Hour.ToString();
                row[4] = date.Minute.ToString();
                row[5] = date.Second.ToString();
                row[6] = date.Millisecond.ToString();
                row[7] = value.Past50s.ToString();
                row[8] = value.Past30s.ToString();
                row[9] = value.Past10s.ToString();
                row[10] = value.Predicted50sAhead.ToString();
                a.AddRow(row);
            }
            a.Save();
        }
    }
}
