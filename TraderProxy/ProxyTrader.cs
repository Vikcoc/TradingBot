using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TradingWebSocket.BaseTrader;

namespace TraderProxy
{
    public sealed class ProxyTrader : ITrader
    {
        private readonly ITrader _trader;
        private readonly ProxyEfDbContext _context;

        public static async Task<ProxyTrader> GetInstance(ITrader trader, ProxyEfDbContext context)
        {
            await context.Database.MigrateAsync();
            return new ProxyTrader(trader, context);
        }

        private ProxyTrader(ITrader trader, ProxyEfDbContext context)
        {
            _trader = trader;
            _context = context;

            _trader.PriceUpdate += SavePrice;
        }

        public IPriceUpdate? Price => _trader.Price;
        private double _price;
        private async Task SavePrice(IPriceUpdate price)
        {
            try
            {
                if(price.Actual == _price)
                    return;
                _price = price.Actual!.Value;
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT [TradingBot].[dbo].[MarketStateSnaps] ([DateTime], [Trade], [TradeName], [InstrumentName], [BestBid], [BestAsk], [Actual], [Low], [High], [Volume], [Change], [Timestamp], [BigVolume], [PartChange]) VALUES (@DateTime, @Trade, @TradeName, @InstrumentName, @BestBid, @BestAsk, @Actual, @Low, @High, @Volume, @Change, @Timestamp, @BigVolume, @PartChange)",
                    new SqlParameter("@DateTime", DateTime.UtcNow),
                    new SqlParameter("@Trade", Trade),
                    new SqlParameter("@TradeName", Trade.ToString()),
                    new SqlParameter("@InstrumentName", price.InstrumentName),
                    new SqlParameter("@BestBid", price.BestBid),
                    new SqlParameter("@BestAsk", price.BestAsk),
                    new SqlParameter("@Actual", price.Actual),
                    new SqlParameter("@Low", price.Low),
                    new SqlParameter("@High", price.High),
                    new SqlParameter("@Volume", price.Volume),
                    new SqlParameter("@Change", price.Change),
                    new SqlParameter("@Timestamp", price.Timestamp),
                    new SqlParameter("@BigVolume", price.BigVolume),
                    new SqlParameter("@PartChange", price.PartChange ?? default));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public double? BuyAvailable => _trader.BuyAvailable;
        public double? SellAvailable => _trader.SellAvailable;

        public Trades Trade => _trader.Trade;

        public event Func<IPriceUpdate, Task> PriceUpdate
        {
            add => _trader.PriceUpdate += value;
            remove => _trader.PriceUpdate -= value;
        }
        public event Func<double, Task> BuyAvailableUpdate
        {
            add => _trader.BuyAvailableUpdate += value;
            remove => _trader.BuyAvailableUpdate -= value;
        }
        public event Func<double, Task> SellAvailableUpdate
        {
            add => _trader.SellAvailableUpdate += value; 
            remove => _trader.SellAvailableUpdate -= value;
        }

        public async Task<bool> Buy(double amount) => await _trader.Buy(amount);

        public async Task<bool> Sell(double amount) => await _trader.Sell(amount);

        public async Task Start(Trades trade) => await _trader.Start(trade);
    }
}
