using Application.Consumers.Configuration;
using Application.Extentions;
using Application.Profiles;
using Application.Requests;
using Common.MediatR.Commands;
using Domain.Contracts;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using Domain.Interfaces.Common;
using FluentMigrator.Runner;
using Infra.Data.Context;
using Infra.Data.Repositories;
using Infra.Data.Repositories.Base;
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
    public static void AddDependenceInjectionConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("DbConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        #region ContainerDI
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAlunoRepository, AlunoRepository>();
        #endregion

        #region Migration
        services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(configuration.GetConnectionString("DbConnection"))
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());
        #endregion

        services.AddAutoMapper(typeof(AlunoProfile));

        services.AddMediatR(typeof(AlunoRequest));

        #region MediatR
        services.AddScoped<IRequestHandler<PutEntityCommand<Aluno>, Aluno>, PutEntityCommandHandler<Aluno>>();
        #endregion
    }

    public static void AddMassTransientConsumer(this IServiceCollection services, IConfiguration configuration)
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
                    cfg.Host("localhost", "/", h =>
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
                                    TimeSpan.FromSeconds(30)));

                    cfg.Message<AlunoContract>(x => x.SetEntityName(TopicNames.AlunoTopic.EnviromentName()));

                    cfg.ConfigureEndpoints(context, new DefaultEndpointNameFormatter("dev-", false));
                });
            });
        }
        #endregion
    }

    public static void AddDependenceInjectionProducer(this IServiceCollection services, IConfiguration configuration)
    {
        #region ContainerDI
        services.AddScoped(typeof(IEventBusInterface<>), typeof(EventBus<>));
        #endregion

        #region MassTransient
        if (Boolean.Parse(configuration["MassTransient:enable"]))
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username(configuration["AccessKey"]);
                        h.Password(configuration["SecretKey"]);
                    });

                    cfg.Message<AlunoContract>(x => x.SetEntityName(TopicNames.AlunoTopic.EnviromentName()));
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
        #endregion
    }
}