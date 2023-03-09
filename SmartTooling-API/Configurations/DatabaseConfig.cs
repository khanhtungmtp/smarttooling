using Microsoft.EntityFrameworkCore;
using SmartTooling_API.Data;

namespace SmartTooling_API.Configurations
{
    public static class DatabaseConfig
    {
        public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration) {
            if (services == null) throw new ArgumentNullException(nameof(services));

            string factory = configuration.GetSection("AppSettings:Factory").Value;
            string area = configuration.GetSection("AppSettings:Area").Value;
            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString($"{factory}_{area}_Connection")));
            services.AddDbContext<SHCDataContext>(options => options.UseSqlServer(configuration.GetConnectionString($"SHC_{area}_Connection")));
            services.AddDbContext<CBDataContext>(options => options.UseSqlServer(configuration.GetConnectionString($"CB_{area}_Connection")));
            services.AddDbContext<SPCDataContext>(options => options.UseSqlServer(configuration.GetConnectionString($"SPC_{area}_Connection")));
            services.AddDbContext<TSHDataContext>(options => options.UseSqlServer(configuration.GetConnectionString($"TSH_{area}_Connection")));
        }
    }
}