using Cache.Settings;
using EasyCaching.Core.Configurations;
using EasyCaching.Serialization.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Cache.Configuration;

public static class RedisConfiguration
{
    private const string RedisKey = "RedisCache";
    private const string LocalKey = "memoryLocal";
    private const int DatabaseKey = 0;
    private const string JsonKey = "jsonSerializer";
    private const string EasycachingKey = "easycaching_setting";
    public static void AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var redis = configuration.GetSection("Redis").Get<RedisSettings>();


        services.AddEasyCaching(options =>
        {
            // local
            //options.UseInMemory(LocalKey);
            // distributed
            options.UseRedis(config =>
            {
                config.DBConfig.Endpoints.Add(new ServerEndPoint(redis.Address, redis.Port));
                config.DBConfig.Password = redis.Password;
                config.DBConfig.Database = DatabaseKey;
                config.DBConfig.IsSsl = false;
                config.DBConfig.AbortOnConnectFail = false;
                config.SerializerName = JsonKey;
            }, RedisKey);

            // with a default name [json]
            options.WithJson();

            // with a custom name [myname]
            options.WithJson(JsonKey);

            // add some serialization settings
            Action<EasyCachingJsonSerializerOptions> easycaching = x =>
            {
                x.NullValueHandling = NullValueHandling.Ignore;
            };

            options.WithJson(easycaching, EasycachingKey);
        });
    }
}
