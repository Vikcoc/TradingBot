using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataExporter.Scripts
{
    public class Script6
    {
        public static void Run()
        {
            var connection = new SqlConnection("Server=DESKTOP-U28TOVR;Database=TradingBot;Trusted_Connection=True;");
            const string query = "SELECT a.[DateTime], a.[Actual], ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -55, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -45, a.DateTime) ORDER BY b.DateTime DESC ) as Past50s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -35, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -25, a.DateTime) ORDER BY b.DateTime DESC ) as Past30s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -15, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -5, a.DateTime) ORDER BY b.DateTime DESC ) as Past10s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, 45, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, 55, a.DateTime) ORDER BY b.DateTime DESC ) as Predicted50sAhead FROM [dbo].[MarketStateSnaps] a WHERE '2023-03-18 00:00:00.0'< a.DateTime ORDER BY a.DateTime";
            var values = connection.Query<dynamic>(query).ToList();
            var a = new SimpleCsvWriter("ma-ta6.csv");

            Console.WriteLine("GotData");
            Console.WriteLine(values.Count);

            values = values.Where(value =>
                value.Past50s != null && value.Past30s != null && value.Past10s != null &&
                value.Predicted50sAhead != null).ToList();

            Console.WriteLine("Filtered data");
            Console.WriteLine(values.Count);

            //a.AddRow(new[] { "Past50s", "Past30s", "Past10s", "Predicted50sAhead" });
            foreach (var value in values)
            {
                var act = (double)value.Actual;

                var row = new string[4];
                //row[0] = value.Actual.ToString();
                row[0] = ((double)value.Past50s - act).ToString(CultureInfo.InvariantCulture);
                row[1] = ((double)value.Past30s - act).ToString(CultureInfo.InvariantCulture);
                row[2] = ((double)value.Past10s - act).ToString(CultureInfo.InvariantCulture);
                row[3] = ((double)value.Predicted50sAhead - act).ToString(CultureInfo.InvariantCulture);
                a.AddRow(row);
            }
            a.Save();
        }
    }

}
