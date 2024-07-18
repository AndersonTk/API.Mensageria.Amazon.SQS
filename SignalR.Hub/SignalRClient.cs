using Domain.Contracts;
using Domain.Entities;
using Domain.Interfaces.Common;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.Hub;
public class SignalRClient
{
    private readonly HubConnection _connection;
    private readonly IEventBusInterface<ProductContract> _bus;

    public SignalRClient(IEventBusInterface<ProductContract> bus)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7195/connectionHub")
            .Build();

        _bus = bus;

        _connection.On<List<Product>>("ReceivedProducts", (products) =>
        {
            Console.WriteLine("Products received:");
            foreach (var product in products)
            {
                _bus.PublishMessage(new ProductContract
                {
                    CategoryId = product.CategoryId,
                    CreateUser = product.CreateUser,
                    Id = product.Id,
                    Name = product.Name
                });
            }
        });
    }

    public async Task StartAsync()
    {
        try
        {
            await _connection.StartAsync();
            Console.WriteLine("SignalR connection started.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
        }
    }

    public async Task StopAsync()
    {
        try
        {
            await _connection.StopAsync();
            Console.WriteLine("SignalR connection stopped.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error stopping SignalR connection: {ex.Message}");
        }
    }

    public async Task GetListProductsAsync()
    {
        if (_connection.State == HubConnectionState.Connected)
        {
            await _connection.InvokeAsync("GetListProducts");
        }
    }
}