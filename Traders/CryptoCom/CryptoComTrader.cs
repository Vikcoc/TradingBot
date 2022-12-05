using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Traders.CryptoCom.Data;
using Traders.CryptoCom.Dto;
using Traders.CryptoCom.Socket;
using TradingWebSocket.BaseTrader;

namespace Traders.CryptoCom
{
    public class CryptoComTrader : ITrader
    {
        protected readonly CryptoComMarketAdapter MarketAdapter;
        protected readonly CryptoComUserAdapter UserAdapter;

        public Trades Trade { get; private set; }

        public CryptoComTrader(CryptoComMarketAdapter marketAdapter, CryptoComUserAdapter userAdapter)
        {
            MarketAdapter = marketAdapter;
            UserAdapter = userAdapter;
        }

        public async Task Start(Trades trade)
        {
            if (Trade != default)
                throw new NotSupportedException("Cannot reconfigure " + this.GetType().Name);
            Trade = trade;
            var tickerFactory = new CryptoComSubscriptionResponseFactory<CryptoComSubscriptionTickerData>(Trade);
            tickerFactory.OnValidObject += async response =>
            {
                if (PriceUpdate != null && response.Result is { Data: { } })
                {
                    foreach (var data in response.Result.Data.Where(data => data.Actual != null))
                    {
                        Price = data.Actual!.Value;
                        await PriceUpdate(data.Actual!.Value);
                    }
                }
            };
            MarketAdapter.AddSocketResponse(tickerFactory);
            var balanceFactory = new CryptoComResponseFactory<CryptoComSubscriptionResponse<CryptoComSubscriptionBalanceData>>();
            balanceFactory.OnValidObject += async response =>
            {
                if(response.Result is { Data: { } })
                {
                    var first = response.Result.Data.FirstOrDefault(x => x.Currency == CryptoComTrades.Trades[Trade].FirstCurrency);
                    if (first != null)
                    {
                        BuyAvailable = first.Available;
                        if (BuyAvailableUpdate != null)
                            await BuyAvailableUpdate(first.Available);
                    }
                    var second = response.Result.Data.FirstOrDefault(x => x.Currency == CryptoComTrades.Trades[Trade].SecondCurrency);
                    if (second != null)
                    {
                        SellAvailable = second.Available;
                        if (SellAvailableUpdate != null)
                            await SellAvailableUpdate(second.Available);
                    }
                }
            };
            UserAdapter.AddSocketResponse(balanceFactory);
            await MarketAdapter.ConnectAndListen();
            await UserAdapter.ConnectAndListen();
            await UserAdapter.Send(new CryptoComSubscriptionRequest
            {
                Channels = new List<string>
                {
                    CryptoComMethods.Balance,
                }
            });
            await MarketAdapter.Send(new CryptoComSubscriptionRequest
            {
                Channels = new List<string>
                {
                    CryptoComMethods.Ticker + "." + CryptoComTrades.Trades[trade].Trade,
                }
            });
        }

        public event Func<double, Task>? PriceUpdate;
        public event Func<double, Task>? BuyAvailableUpdate;
        public event Func<double, Task>? SellAvailableUpdate;
        public double Price { get; set; }
        public double BuyAvailable { get; set; }
        public double SellAvailable { get; set; }
        public async Task<bool> Buy(double amount)
        {
            await UserAdapter.Send(new CryptoComOrderRequest
            {
                InstrumentName = CryptoComTrades.Trades[Trade],
                Side = "BUY",
                Type = "MARKET",
                Quantity = amount,
            });
            return true;
        }

        public async Task<bool> Sell(double amount)
        {
            await UserAdapter.Send(new CryptoComOrderRequest
            {
                InstrumentName = CryptoComTrades.Trades[Trade],
                Side = "SELL",
                Type = "MARKET",
                Quantity = amount,
            });
            return true;
        }
    }
}
