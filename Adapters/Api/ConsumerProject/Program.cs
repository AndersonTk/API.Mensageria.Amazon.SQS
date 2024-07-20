using Infra.Data.Context;
using Infra.Ioc.Infraestructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using SignalR_Hub;
using SqlTableDependency.ServiceBroken.Configuration;
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

app.ConfigureAppConsumer();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

app.MapHub<ConnectHub>("connectionHub");
app.UseTableDependency(builder.Configuration);

app.MapControllers();

app.Run();
