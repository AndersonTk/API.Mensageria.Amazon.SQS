using Domain.Extensions;
using Hangfire;
using Infra.Data.Context;
using Infra.Ioc.Configuration.Swagger;
using Infra.Ioc.Infraestructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Serilog.Sinks.LogBee.AspNetCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.ConfigureConsumer(builder.Configuration, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
dbContext.Database.Migrate();

var provider = app.Services.GetService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{

}

app.UseApiVersioning();

app.UseMiddleware<SwaggerAuthorizeMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseLogBeeMiddleware();

app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapHangfireDashboard("/hangfire");
});

app.MapControllers();

app.Run();
