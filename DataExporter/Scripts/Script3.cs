using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataExporter.Scripts
{
    public static class Script3
    {
        public static void Run()
        {
            var connection = new SqlConnection("Server=DESKTOP-U28TOVR;Database=TradingBot;Trusted_Connection=True;");
            const string query = "SELECT [DateTime], [BestBid], [BestAsk], [Actual], [Low], [High] FROM [MarketStateSnaps] ORDER BY [DateTime]";
            var values = connection.Query<dynamic>(query).ToList();
            var a = new SimpleCsvWriter("ma-ta3.csv");
            {
                a.AddRow(new[] { "CurrentValue", "BestBid", "BestAsk", "LowOfTheDay", "HighOfTheDay", "DayOfMonth", "DayOfWeek", "Hour", "Minute", "Second", "Millisecond", "Past50s", "Past30s", "Past10s", "Predicted50sAhead" });
                foreach (var value in values)
                {

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

                    var fr = values.FirstOrDefault(x => date.AddSeconds(48) < (DateTime)x.DateTime
                                                        && (DateTime)x.DateTime < date.AddSeconds(52));

                    var pst = values.FirstOrDefault(x => date.AddSeconds(-52) < (DateTime)x.DateTime
                                                        && (DateTime)x.DateTime < date.AddSeconds(-48));
                    var pst2 = values.FirstOrDefault(x => date.AddSeconds(-32) < (DateTime)x.DateTime
                                                        && (DateTime)x.DateTime < date.AddSeconds(-28));
                    var pst3 = values.FirstOrDefault(x => date.AddSeconds(-12) < (DateTime)x.DateTime
                                                        && (DateTime)x.DateTime < date.AddSeconds(-8));

                    //Console.WriteLine("For time {0} future {1}, past {2}, past2 {3}, past3 {4} ", date, fr?.Actual, pst?.Actual, pst2?.Actual, pst3?.Actual);

                    if (fr == null || pst == null || pst2 == null || pst3 == null)
                        continue;

                    row[11] = pst.Actual.ToString();
                    row[12] = pst2.Actual.ToString();
                    row[13] = pst3.Actual.ToString();
                    row[14] = fr.Actual.ToString();

                    a.AddRow(row);
                }
                a.Save();
            }
        }
    }
}
