using Infra.Data.Context;
using Infra.Ioc.Infraestructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using SignalR.Hub.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.ConfigureProducer(builder.Configuration, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ProducerDbContext>();
dbContext.Database.Migrate();

var provider = app.Services.GetService<IApiVersionDescriptionProvider>();

app.ConfigureAppProducer();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

app.MapControllers();

app.Run();
