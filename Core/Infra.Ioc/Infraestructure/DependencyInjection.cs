using Application.Profiles;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using Domain.Interfaces.Common;
using FluentMigrator.Runner;
using Infra.Data.Context;
using Infra.Data.Repositories;
using Infra.Data.Repositories.Base;
using Infra.Data.Repositories.Common;
using Infra.Ioc.Configuration.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SignalR.Hub;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Infra.Ioc.Infraestructure;

public static class DependencyInjection
{
    public static void AddDependenceInjectionConsumer(this IServiceCollection services, IConfiguration configuration)
    {
        #region ContainerDI
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IEventBusInterface, EventBus>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureConsumerSwaggerOptions>();

        #endregion

        #region Migration
        services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                .AddSqlServer()
                .WithGlobalConnectionString(configuration.GetConnectionString("DbConnection"))
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole());
        #endregion

        services.AddAutoMapper(typeof(CategoryProfile));
    }

    public static void AddDependenceInjectionProducer(this IServiceCollection services)
    {
        #region ContainerDI
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IEventBusInterface, EventBus>();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureProducerSwaggerOptions>();
        services.AddScoped<SignalRClient>();
        #endregion
    }
}