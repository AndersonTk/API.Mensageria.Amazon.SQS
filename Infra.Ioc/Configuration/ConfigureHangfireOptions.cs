using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Ioc.Configuration;
public static class ConfigureHangfireOptions
{
    public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(x =>
        {
            x.UseSqlServerStorage(configuration.GetConnectionString("DbConnection"));
            x.UseRecommendedSerializerSettings();
            x.UseMemoryStorage();
        });

        services.AddHangfireServer(options =>
        {
            options.ServerName = Environment.MachineName + "_" + "conusmer-project";
            options.Queues = new[] { "consumers" };
        });

        return services;
    }
}
