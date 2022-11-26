using System.Collections.Generic;
using Newtonsoft.Json;

namespace Traders.CryptoCom.Dto;

public class CryptoComSubscriptionInfo<T>
{
    [JsonProperty("instrument_name")]
    public string InstrumentName { get; set; } = string.Empty;
    [JsonProperty("subscription")]
    public string Subscription { get; set; } = string.Empty;
    [JsonProperty("channel")]
    public string Channel { get; set; } = string.Empty;
    [JsonProperty("data")]
    public List<T>? Data { get; set; }
}