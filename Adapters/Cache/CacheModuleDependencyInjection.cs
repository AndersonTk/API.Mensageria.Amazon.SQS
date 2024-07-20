using Cache.Configuration;
using Domain.Interfaces.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cache;

public static class CacheModuleDependencyInjection
{
    public static void AddCacheModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRedisConfiguration(configuration);

        services.AddTransient<ICache, CacheRedis>();
    }
}