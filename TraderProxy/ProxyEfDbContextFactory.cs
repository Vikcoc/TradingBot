using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TraderProxy
{
    public class ProxyEfDbContextFactory : IDesignTimeDbContextFactory<ProxyEfDbContext>
    {
        /// <summary>
        /// Get context to make migrations
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProxyEfDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder x = new DbContextOptionsBuilder();
            x.UseSqlServer("");
            return new ProxyEfDbContext(x.Options);
        }
    }
}
