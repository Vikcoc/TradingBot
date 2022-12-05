using System.ComponentModel.DataAnnotations;
using TradingWebSocket.BaseTrader;

namespace TraderProxy.Entities
{
    public class MarketStateSnap
    {
        [Key]
        public DateTime DateTime { get; set; }
        public Trades Trade { get; set; }
        [DataType("varchar(20)")]
        public string TradeName { get; set; } = string.Empty;
        public double Price { get; set; }

    }
}