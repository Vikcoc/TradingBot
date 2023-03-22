﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataExporter.Scripts
{
    public class Script11
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
            var a = new SimpleCsvWriter("ma-ta14.csv");

            Console.WriteLine("GotData");
            Console.WriteLine(values.Count);

            //var arrValues = values.ToList();

            //Console.WriteLine("Filtered data");
            //Console.WriteLine(values.Count);

            //var min = values.Min(x => x.Actual);
            //var max = values.Max(x => x.Actual);

            //Console.WriteLine("Min {0}", min);
            //Console.WriteLine("Max {0}", max);

            //a.AddRow(new[] { "Past50s", "Past30s", "Past10s", "Predicted50sAhead" });

            //var norm = GetMinMaxNorm(min, max);

            var days = values.GroupBy(x => x.DateTime.Day);

            foreach (var day in days)
            {
                for (var i = 51; i < day.Count() - 6; i++)
                {
                    var cur = day.Skip(i).First();
                    var fut = day.Skip(i + 5).First().Actual;
                    var past = day.Skip(i - 21).Take(20).Select(x => x.Actual - cur.Actual).ToList();
                    var fav = fut > cur.Actual + 0.3M ? 1 : fut < cur.Actual - 0.3M ? -1 : 0;
                    a.AddRow(past
                        .Append(cur.DateTime.Minute)
                        .Append(fav)
                        .Select(x => x.ToString(CultureInfo.InvariantCulture)));
                    //a.AddRow(past.Zip(past.Skip(1).Select(x => x))
                    //    //.SkipLast(1)
                    //    .Select(x => x.Second - x.First)
                    //    .Append(cur.Actual)
                    //    .Append(cur.DateTime.Minute)
                    //    .Append(fav)
                    //    .Select(x => x.ToString(CultureInfo.InvariantCulture)));
                    //a.AddRow(new []
                    //{
                    //    past.Zip(past.Skip(1).Select(x => x)).Select(x => x.Second - x.First).Average(),
                    //    cur.DateTime.Minute,
                    //    fav

                    //}.Select(x => x.ToString(CultureInfo.InvariantCulture)));
                }
            }
            
            a.Save();
        }
    }
}
