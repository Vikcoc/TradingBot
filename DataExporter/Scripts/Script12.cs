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
    public class Script12
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
            var a = new SimpleCsvWriter("ma-ta15.csv");

            Console.WriteLine("GotData");
            Console.WriteLine(values.Count);

            
            

            foreach (var value in values)
            {
                
                a.AddRow(new[] {value.DateTime.ToString(CultureInfo.InvariantCulture), value.Actual.ToString(CultureInfo.InvariantCulture) });
                    
            }

            a.Save();
        }
    }
}
