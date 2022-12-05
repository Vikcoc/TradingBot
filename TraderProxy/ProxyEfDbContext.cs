using Microsoft.EntityFrameworkCore;
using TraderProxy.Entities;

namespace TraderProxy
{
    public class ProxyEfDbContext : DbContext
    {
        public ProxyEfDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<MarketStateSnap> MarketStateSnaps { get; set; }
    }
}
