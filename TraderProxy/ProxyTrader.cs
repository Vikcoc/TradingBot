using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TradingWebSocket.BaseTrader;

namespace TraderProxy
{
    public sealed class ProxyTrader : ITrader
    {
        private readonly ITrader _trader;
        private readonly IServiceProvider _serviceProvider;

        public static async Task<ProxyTrader> GetInstance(ITrader trader, IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ProxyEfDbContext>();
            await context.Database.MigrateAsync();
            return new ProxyTrader(trader, serviceProvider);
        }

        private ProxyTrader(ITrader trader, IServiceProvider serviceProvider)
        {
            _trader = trader;
            _serviceProvider = serviceProvider;

            _trader.PriceUpdate += SavePrice;
        }

        public double Price { get => _trader.Price; }

        private async Task SavePrice(double price)
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<ProxyEfDbContext>();
            //var s = < ![CDATA[ async ]]>
            context.Database.SqlQueryRaw<int>("",
                new SqlParameter("DateTime", DateTime.UtcNow));
            await context.MarketStateSnaps.AddAsync(new Entities.MarketStateSnap
            {
                DateTime = DateTime.UtcNow,
                Price = price,
                Trade = _trader.Trade,
                TradeName = _trader.Trade.ToString(),
            });
            await context.SaveChangesAsync();
        }

        public double BuyAvailable => _trader.BuyAvailable;
        public double SellAvailable => _trader.SellAvailable;

        public Trades Trade => _trader.Trade;

        public event Func<double, Task> PriceUpdate
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
