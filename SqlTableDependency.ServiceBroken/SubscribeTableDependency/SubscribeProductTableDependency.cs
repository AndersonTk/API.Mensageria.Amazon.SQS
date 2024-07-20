using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Interfaces.Common;
using Microsoft.Extensions.Configuration;
using SqlTableDependency.ServiceBroken.Base;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;
using ErrorEventArgs = TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs;

namespace SqlTableDependency.ServiceBroken.SubscribeTableDependency;
public class SubscribeProductTableDependency : SubscribeTableBase
{
    SqlTableDependency<Product> _tableProductDependency;

    public SubscribeProductTableDependency(IEventBusInterface bus, IMapper mapper) : base(bus, mapper)
    {
    }

    public void SubscribeProductDependency(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        var connectionString2 = "Data Source=localhost;Initial Catalog=Banco_Estudos_Mensageria_Producer;User ID=sa;Password=62745263;";

        var mapper = new ModelToTableMapper<Product>();
        mapper.AddMapping(c => c.Id, "ProductId");

        _tableProductDependency = new SqlTableDependency<Product>(connectionString2, mapper: mapper);
        _tableProductDependency.OnChanged += TableProduct_Onchange;
        _tableProductDependency.OnError += TableProduct_OnError;
        _tableProductDependency.Start();
    }

    private void TableProduct_Onchange(object sender, RecordChangedEventArgs<Product> e)
    {
        var changeTypes = new List<ChangeType> { ChangeType.Insert, ChangeType.Update };

        if (changeTypes.Contains(e.ChangeType))
            _bus.PublishMessage(_mapper.Map<ProductContract>(e.Entity));

        if (e.ChangeType == ChangeType.Delete)
            _bus.PublishMessage(_mapper.Map<ProductDeleteContract>(e.Entity));
    }

    private void TableProduct_OnError(object sender, ErrorEventArgs e)
    {
        Console.WriteLine($"{nameof(Product)} SqlTableDependence error: {e.Error.Message}");
        throw new Exception($"{nameof(Product)} SqlTableDependence error: {e.Error.Message}");
    }
}