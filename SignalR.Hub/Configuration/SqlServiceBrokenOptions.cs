using Microsoft.Extensions.DependencyInjection;
using SqlTableDependency.ServiceBroken.SubscribeTableDependency;

namespace SqlTableDependency.ServiceBroken.Configuration;
public static class SqlServiceBrokenOptions
{
    public static IServiceCollection ConfigureServiceBrokerDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<SubscribeProductTableDependency>();
        services.AddScoped<SubscribeCategoryTableDependency>();
        return services;
    }
}
