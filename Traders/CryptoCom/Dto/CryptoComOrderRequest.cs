using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Channels;
using Traders.CryptoCom.Data;
using TradingWebSocket.Adapter;

namespace Traders.CryptoCom.Dto
{
    internal class CryptoComOrderRequest : ITransaction
    {
        public string InstrumentName { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public double? Price { get; set; }
        public double? Quantity { get; set; }
        public double? Notional { get; set; }
        public string ClientOid { get; set; } = string.Empty;
        public string TimeInForce { get; set; } = string.Empty;
        public string ExecInst { get; set; } = string.Empty;
        public string TriggerPrice { get; set; } = string.Empty;

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new CryptoComParamRequest
            {
                Method = CryptoComMethods.CreateOrder,
                Params = new Dictionary<string, object>
                {
                    {"instrument_name", InstrumentName},
                    {"side", Side},
                    {"type", Type},
                    //{"price", Price!},
                    {"quantity", Quantity!},
                    //{"notional", Notional!},
                    //{"client_oid", ClientOid},
                    //{"time_in_force", TimeInForce},
                    //{"exec_inst", ExecInst},
                    //{"trigger_price", TriggerPrice},
                }
            });
        }
    }
}