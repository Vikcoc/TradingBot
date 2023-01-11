using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

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
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("connection.json", true)
                .Build();
            DbContextOptionsBuilder x = new DbContextOptionsBuilder();
            x.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new ProxyEfDbContext(x.Options);
        }
    }
}
