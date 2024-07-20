using Application.Consumers.Configuration;
using Application.Extentions;
using Application.Profiles;
using Domain.Contracts;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infra.Ioc.Infraestructure;

public static class DependencyInjectionMassTransit
{
    public static void AddMassTransitConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        #region MassTransient
        if (Boolean.Parse(configuration["MassTransient:enable"]))
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.Load("Application"));

                x.SetKebabCaseEndpointNameFormatter();

                x.AddDelayedMessageScheduler();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMq:url"], "/", h =>
                    {
                        h.Username(configuration["AccessKey"]);
                        h.Password(configuration["SecretKey"]);
                    });


                    cfg.UseDelayedMessageScheduler();

                    cfg.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });

                    cfg.UseMessageRetry(a => a.Incremental(3,
                                    TimeSpan.FromSeconds(30),
                                    TimeSpan.FromSeconds(10)));

                    cfg.Message<CategoryContract>(x => x.SetEntityName(TopicNames.CategoryTopic.EnviromentName()));
                    cfg.Message<CategoryDeleteContract>(x => x.SetEntityName(TopicNames.CategoryDeleteTopic.EnviromentName()));
                    cfg.Message<ProductContract>(x => x.SetEntityName(TopicNames.ProductTopic.EnviromentName()));
                    cfg.Message<ProductDeleteContract>(x => x.SetEntityName(TopicNames.ProductDeleteTopic.EnviromentName()));

                    cfg.ConfigureEndpoints(context, new DefaultEndpointNameFormatter($"{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower()}-", false));
                });
            });
        }
        #endregion
    }

    public static void AddMassTransitProducer(this IServiceCollection services, IConfiguration configuration)
    {
        #region ContainerDI
        #endregion

        services.AddAutoMapper(typeof(CategoryProfile));

        #region MassTransient
        if (Boolean.Parse(configuration["MassTransient:enable"]))
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMq:url"], "/", h =>
                    {
                        h.Username(configuration["AccessKey"]);
                        h.Password(configuration["SecretKey"]);
                    });

                    cfg.Message<CategoryContract>(x => x.SetEntityName(TopicNames.CategoryTopic.EnviromentName()));
                    cfg.Message<ProductContract>(x => x.SetEntityName(TopicNames.ProductTopic.EnviromentName()));
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
        #endregion
    }
}
