using Application.Configuration;
using Application.Extentions;
using Application.Profiles;
using Application.Requests;
using Domain.Contracts;
using Domain.Interfaces;
using Domain.Interfaces.Common;
using FluentMigrator.Runner;
using Infra.Data.Context;
using Infra.Data.Repositories;
using Infra.Data.Repositories.Common;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infra.Ioc.Infraestructure;

public static class DependencyInjection
{
    public static void AddDependenceInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DbConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        #region ContainerDI
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IEventBusInterface<>), typeof(EventBus<>));
        services.AddScoped<IAlunoRepository, AlunoRepository>();
        #endregion

        services.AddAutoMapper(typeof(AlunoProfile));

        services.AddMediatR(typeof(AlunoRequest));
    }

    public static void AddDependenceInjectionConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        #region MassTransient
        services.AddMassTransit(x =>
        {
            //x.AddConsumer<AlunoConsumer, AlunoConsumerDefinition>();
            x.AddConsumers(Assembly.Load("Application"));

            x.SetKebabCaseEndpointNameFormatter();

            x.AddDelayedMessageScheduler();

            x.UsingAmazonSqs((context, cfg) =>
            {
                cfg.Host("us-east-2", h =>
                {
                    h.AccessKey("AKIA4USGFP4VVQTRFBLO");
                    h.SecretKey("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");
                    h.Scope("dev", true);
                });

                if (Boolean.Parse(configuration["RabbitMq:enable"]))
                {
                    cfg.UseDelayedMessageScheduler();

                    cfg.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });

                    cfg.UseMessageRetry(a => a.Incremental(3,
                                    TimeSpan.FromSeconds(1),
                                    TimeSpan.FromSeconds(1)));
                    //cfg.UseDelayedRedelivery(x => x.Incremental(3, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)));
                    //cfg.UseMessageRetry(a => a.Immediate(5));

                    cfg.Message<AlunoContract>(x => x.SetEntityName(TopicNames.AlunoTopic.EnviromentName()));

                    cfg.ConfigureEndpoints(context, new DefaultEndpointNameFormatter("dev-", false));

                    //cfg.ReceiveEndpoint(Environment.MachineName + "-aluno-queue", ep =>
                    //{
                    //    ep.ConcurrentMessageLimit = 2;
                    //    ep.ConfigureConsumer<AlunoConsumer>(context);
                    //    ep.UseMessageRetry(a => a.Incremental(3,
                    //                TimeSpan.FromSeconds(5),
                    //                TimeSpan.FromSeconds(5)));
                    //});
                }
            });
        });

        #endregion

        #region Migration
        services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(configuration.GetConnectionString("DbConnection"))
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());
        #endregion
    }

    public static void AddDependenceInjectionProducer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingAmazonSqs((context, cfg) =>
            {
                cfg.Host("us-east-2", h =>
                {
                    h.AccessKey("AKIA4USGFP4VVQTRFBLO");
                    h.SecretKey("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");
                });

                cfg.Message<AlunoContract>(x => x.SetEntityName(TopicNames.AlunoTopic.EnviromentName()));
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}