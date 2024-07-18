using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Ioc.Configuration;

public static class ConfigureHangfireOptions
{
    public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            config.UseSimpleAssemblyNameTypeSerializer();
            config.UseRecommendedSerializerSettings();
            config.UseSqlServerStorage(configuration.GetConnectionString("DbConnection"));
        });

        services.AddHangfireServer(options =>
        {
            options.ServerName = Environment.MachineName + "_" + "conusmer-project";
            options.Queues = new[] { "produtos" };
            options.WorkerCount = 1;
        });

        return services;
    }
}
