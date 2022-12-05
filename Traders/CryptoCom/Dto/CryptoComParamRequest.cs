using System.Collections.Generic;
using Newtonsoft.Json;
using Traders.CryptoCom.Data;

namespace Traders.CryptoCom.Dto;

internal class CryptoComParamRequest : CryptoComBaseRequest
{
    [JsonProperty("params")]
    public Dictionary<string, object>? Params { get; set; }
}