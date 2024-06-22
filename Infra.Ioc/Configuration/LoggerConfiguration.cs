using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.LogBee;

namespace Infra.Ioc.Configuration;

public static class LoggerConfiguration
{
    public static IServiceCollection AddLoggerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((services, lc) => lc
                     .WriteTo.LogBee(new LogBeeApiKey(
                             configuration["LogBee.OrganizationId"]!,
                             configuration["LogBee.ApplicationId"]!,
                             configuration["LogBee.ApiUrl"]!
         ),
         services
     ));
        return services;
    }
}
