using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Security.Authentication;

namespace Infra.Ioc.Configuration;

public static class RedisConfiguration
{
    public static string ConnectionString { get; private set; }
    
    public static ConnectionMultiplexer Connection
    {
        get
        {
            return _lazyConnection.Value;
        }
    }

    private static readonly Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
    {
        return ConnectionMultiplexer.Connect(ConnectionString);
    });

    public static void AddRedisConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var host = configuration["Redis:Address"]!;
        var port = int.Parse(configuration["Redis:Port"]!);
        var password = configuration["Redis:Password"]!;
        var options = ConfigurationOptions.Parse($"{host}:{port}, abortConnect=false, ssl=false, sslProtocols={SslProtocols.Tls}");
        options.ConnectTimeout = 90000;
        options.SyncTimeout = 90000;
        options.AsyncTimeout = 90000;
        options.AbortOnConnectFail = false;
        options.Password = password;
        ConnectionString = options.ToString();
    }
}
