using Hangfire;
using Hangfire.MemoryStorage;
using Infra.Data.Context;
using Infra.Ioc.Infraestructure;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddDependenceInjectionConsumer(builder.Configuration);
services.AddDependenceInjection(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(x =>
{
    x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DbConnection"));
    x.UseRecommendedSerializerSettings();
    x.UseMemoryStorage();
});

builder.Services.AddHangfireServer(options =>
{
    options.ServerName = Environment.MachineName + "_" + "conusmer-project";
    options.Queues = new[] { "consumers" };
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
dbContext.Database.Migrate();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHangfireDashboard("/hangfire");
});

app.MapControllers();

app.Run();
