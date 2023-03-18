using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace DataExporter.Scripts
{
    public class Script4
    {
        public static void Run()
        {
            var connection = new SqlConnection("Server=DESKTOP-U28TOVR;Database=TradingBot;Trusted_Connection=True;");
            const string query = "SELECT a.[DateTime], a.[BestBid], a.[BestAsk], a.[Actual], a.[Low], a.[High], ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -55, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -45, a.DateTime) ORDER BY b.DateTime DESC ) as Past50s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -35, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -25, a.DateTime) ORDER BY b.DateTime DESC ) as Past30s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, -15, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, -5, a.DateTime) ORDER BY b.DateTime DESC ) as Past10s, ( SELECT TOP (1) [Actual] FROM [dbo].[MarketStateSnaps] b WHERE DATEADD(second, 45, a.DateTime) < b.DateTime AND b.DateTime < DATEADD(second, 55, a.DateTime) ORDER BY b.DateTime DESC ) as Predicted50sAhead FROM [dbo].[MarketStateSnaps] a ORDER BY a.DateTime";
            var values = connection.Query<dynamic>(query).ToList();
            var a = new SimpleCsvWriter("ma-ta4.csv");

            Console.WriteLine("GotData");

            a.AddRow(new[] { "CurrentValue", "BestBid", "BestAsk", "LowOfTheDay", "HighOfTheDay", "DayOfMonth", "DayOfWeek", "Hour", "Minute", "Second", "Millisecond", "Past50s", "Past30s", "Past10s", "Predicted50sAhead" });
            foreach (var value in values)
            {
                //Console.WriteLine(JsonConvert.SerializeObject(value));
                if(value.Past50s == null || value.Past30s == null || value.Past10s == null || value.Predicted50sAhead == null)
                    continue;

                var date = (DateTime)value.DateTime;
                var row = new string[15];
                row[0] = value.Actual.ToString();
                row[1] = value.BestBid.ToString();
                row[2] = value.BestAsk.ToString();
                row[3] = value.Low.ToString();
                row[4] = value.High.ToString();
                row[5] = date.Day.ToString();
                row[6] = date.DayOfWeek.ToString();
                row[7] = date.Hour.ToString();
                row[8] = date.Minute.ToString();
                row[9] = date.Second.ToString();
                row[10] = date.Millisecond.ToString();
                row[11] = value.Past50s.ToString();
                row[12] = value.Past30s.ToString();
                row[13] = value.Past10s.ToString();
                row[14] = value.Predicted50sAhead.ToString();
                a.AddRow(row);
            }
            a.Save();
        }
    }
}
