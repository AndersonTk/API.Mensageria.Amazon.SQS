using Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace SignalR_Hub;
public class ConnectHub : Hub
{
    private readonly IProductRepository _productRepository;

    public ConnectHub(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task GetListProducts()
    {
        var products = await _productRepository.GetAllAsync();
        await Clients.All.SendAsync("ReceivedProducts", products);
    }
}
