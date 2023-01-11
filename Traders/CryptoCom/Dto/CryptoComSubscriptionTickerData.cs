using Newtonsoft.Json;
using Traders.CryptoCom.Data;
using TradingWebSocket.Adapter;
using TradingWebSocket.BaseTrader;

namespace Traders.CryptoCom.Dto
{
    internal class CryptoComSubscriptionTickerData : IResponseDto, IPriceUpdate
    {
        [JsonProperty("i")]
        public string? InstrumentName { get; set; } = string.Empty;

        [JsonProperty("b")]
        public double? BestBid { get; set; }

        [JsonProperty("k")]
        public double? BestAsk { get; set; }

        [JsonProperty("a")]
        public double? Actual { get; set; }

        [JsonProperty("l")]
        public double? Low { get; set; }

        [JsonProperty("h")]
        public double? High { get; set; }

        [JsonProperty("v")]
        public double Volume { get; set; }

        [JsonProperty("c")]
        public double? Change { get; set; }

        [JsonProperty("t")]
        public long Timestamp { get; set; }

        [JsonProperty("vv")]
        public double BigVolume { get; set; }

        [JsonProperty("pc")]
        public double? PartChange { get; set; }

        public static bool CanJson(string json) => json.Contains(CryptoComMethods.Ticker);
    }
}
