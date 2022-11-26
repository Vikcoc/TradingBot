using System.Collections.Generic;
using Newtonsoft.Json;
using TradingWebSocket.Adapter;

namespace Traders.CryptoCom.Dto
{
    public class CryptoComSubscriptionResult<T> where T : IResponseDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("channel")]
        public string Channel { get; set; } = string.Empty;
        [JsonProperty("instrument_name")]
        public string InstrumentName { get; set; } = string.Empty;
        [JsonProperty("subscription")]
        public string Subscription { get; set; } = string.Empty;
        [JsonProperty("data")]
        public List<T>? Data { get; set; }
    }
}
