// See https://aka.ms/new-console-template for more information

using DataExporter;
using System.Data.SqlClient;
using Dapper;
using DataExporter.Scripts;


Script8.Run();
return;
Console.WriteLine("Hello, World!");

var connection = new SqlConnection("Server=DESKTOP-U28TOVR;Database=TradingBot;Trusted_Connection=True;");
var query =
    "SELECT [DateTime], [BestBid], [BestAsk], [Actual], [Low], [High], [Volume], [Change], [BigVolume] FROM [MarketStateSnaps] ORDER BY [DateTime]";
var values = connection.Query<dynamic>(query).ToList();
//using (var a = new SimpleExcelWriter("ma-ta.xlsx"))
//{
//    foreach (var value in values)
//    {
//        var date = (DateTime)((IDictionary<string, object>)value).Values.First();
//        var times = new object[] { date.Day, (int)date.DayOfWeek, date.Hour, date.Minute, date.Second, date.Millisecond};
//        a.AddRow(((IDictionary<string, object>)value).Values.Skip(1).Concat(times).Select(x => x.ToString()));
//    }
//    a.Save();
//}

var a = new SimpleCsvWriter("ma-ta.csv");
{
    foreach (var value in values) {
        var date = (DateTime)((IDictionary<string, object>)value).Values.First();
        var times = new object[] { date.Day, (int)date.DayOfWeek, date.Hour, date.Minute, date.Second, date.Millisecond };
        a.AddRow(((IDictionary<string, object>)value).Values.Concat(times).Select(x => x.ToString()));
    }
    a.Save();
}