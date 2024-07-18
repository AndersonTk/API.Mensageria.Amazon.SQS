using Microsoft.Extensions.DependencyInjection;
using SqlTableDependency.ServiceBroken.Configuration;

namespace SignalR.Hub.Configuration;
public static class ConfigurationSignalRHub
{
    public static IServiceCollection ConfigureServiceBroker(this IServiceCollection services)
    {
        services.ConfigureServiceBrokerDependencyInjection();
        services.AddSignalR();

        return services;
    }
}
