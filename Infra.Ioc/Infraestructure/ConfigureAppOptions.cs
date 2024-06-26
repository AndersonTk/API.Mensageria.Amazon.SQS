using Domain.Extensions;
using Hangfire;
using Infra.Ioc.Configuration.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog.Sinks.LogBee.AspNetCore;

namespace Infra.Ioc.Infraestructure;
public static class ConfigureAppOptions
{
    public static WebApplication ConfigureAppConsumer(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseCors("CorsDevelopment");
        }
        else if (app.Environment.IsProduction())
        {
            app.UseCors("CorsPolicy");
        }

        app.UseApiVersioning();

        app.UseMiddleware<SwaggerAuthorizeMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseLogBeeMiddleware();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHangfireDashboard("/hangfire");
        });

        return app;
    }

    public static WebApplication ConfigureAppProducer(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseCors("CorsDevelopment");
        }
        else if (app.Environment.IsProduction())
        {
            app.UseCors("CorsPolicy");
        }

        app.UseHsts();

        app.UseApiVersioning();

        app.UseMiddleware<SwaggerAuthorizeMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        return app;
    }
}
