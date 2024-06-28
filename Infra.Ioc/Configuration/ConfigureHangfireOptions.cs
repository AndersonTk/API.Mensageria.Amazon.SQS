using Hangfire;
using Hangfire.Redis;
using Hangfire.Tags;
using Hangfire.Tags.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infra.Ioc.Configuration;
public static class ConfigureHangfireOptions
{
    public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedisConnection(configuration);

        var multiplexer = ConnectionMultiplexer.Connect(RedisConfiguration.ConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);

        var options = new RedisStorageOptions
        {
            Prefix = "hangfire:",
            SucceededListSize = 9999,
            DeletedListSize = 4999,
            Db = multiplexer.GetDatabase().Database
        };

        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            config.UseSimpleAssemblyNameTypeSerializer();
            config.UseRecommendedSerializerSettings();
            config.UseRedisStorage(multiplexer, options);
            config.UseTagsWithRedis(multiplexer);
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
