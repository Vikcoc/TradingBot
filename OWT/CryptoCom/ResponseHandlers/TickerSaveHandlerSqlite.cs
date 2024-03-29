﻿using Dapper;
using Newtonsoft.Json.Linq;
using OWT.CryptoCom.Dto;
using System.Data;

namespace OWT.CryptoCom.ResponseHandlers
{
    public class TickerSaveHandlerSqlite : ICryptoComDtoExecutor
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<TickerSaveHandlerSqlite> _logger;



        private const string SelectQuery = "SELECT [Actual] AS Value FROM [NewMarketSnaps] WHERE [Instrument] = 'ETH_USDT' AND [Timestamp] = (SELECT max([Timestamp]) FROM [NewMarketSnaps])";
        private const string InsertQuery = "INSERT INTO [NewMarketSnaps] ([High], [Low], [Actual], [Instrument], [Volume], [UsdVolume], [OpenInterest], [Change], [BestBid],[BestBidSize],[BestAsk],[BestAskSize],[TradeTimestamp],[Timestamp]) VALUES (@High, @Low, @Actual, @Instrument, @Volume, @UsdVolume,@OpenInterest,@Change,@BestBid,@BestBidSize,@BestAsk,@BestAskSize,@TradeTimestamp,@Timestamp)";

        public TickerSaveHandlerSqlite(IDbConnection connection, ILogger<TickerSaveHandlerSqlite> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public bool CanExecute(JObject dto) => dto["method"]?.ToString() == "subscribe" && dto["result"]?["channel"]?.ToString() == "ticker";

        public async Task Execute(JObject dto, CryptoComMarketClient marketClient, CancellationToken token)
        {
            var data = dto?["result"]?["data"]?.ToObject<CryptoComTickerData[]>();
            if (data == null)
            {
                _logger.LogInformation("Received no data from socket");
                return;
            }
            foreach (var item in data)
            {
                if (item.Actual == await _connection.QueryFirstOrDefaultAsync<decimal?>(SelectQuery, new{Instrument = item.Instrument}))
                    continue;
                var time = DateTime.UtcNow;

                await _connection.ExecuteAsync(InsertQuery, new
                {
                    item.High,
                    item.Low,
                    item.Actual,
                    item.Instrument,
                    item.Volume,
                    item.UsdVolume,
                    item.OpenInterest,
                    item.Change,
                    item.BestBid,
                    item.BestBidSize,
                    item.BestAsk,
                    item.BestAskSize,
                    item.TradeTimestamp,
                    Timestamp = time
                });

                // _logger.LogInformation("Inserted {0} for instrument {1} at timestamp {2}", item.Actual, item.Instrument, time);

            }

        }
    }
}
