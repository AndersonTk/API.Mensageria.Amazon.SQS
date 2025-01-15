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
public class SubscribeCategoryTableDependency : SubscribeTableBase
{

    SqlTableDependency<Category> _tableCategoryDependency;
    public SubscribeCategoryTableDependency(IEventBusInterface bus, IMapper mapper) : base(bus, mapper)
    {
    }

    public void SubscribeCategoryDependency(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");

        var mapper = new ModelToTableMapper<Category>();
        mapper.AddMapping(c => c.Id, "CategoryId");

        _tableCategoryDependency = new SqlTableDependency<Category>(connectionString, mapper: mapper);
        _tableCategoryDependency.OnChanged += TableCategory_Onchange;
        _tableCategoryDependency.OnError += TableCategory_OnError;
        _tableCategoryDependency.Start();
    }

    private void TableCategory_Onchange(object sender, RecordChangedEventArgs<Category> e)
    {
        var changeTypes = new List<ChangeType> { ChangeType.Insert, ChangeType.Update };

        if (changeTypes.Contains(e.ChangeType))
            _bus.PublishMessage(_mapper.Map<CategoryContract>(e.Entity));

        if (e.ChangeType == ChangeType.Delete)
            _bus.PublishMessage(_mapper.Map<CategoryDeleteContract>(e.Entity));
    }

    private void TableCategory_OnError(object sender, ErrorEventArgs e)
    {
        Console.WriteLine($"{nameof(Category)} SqlTableDependence error: {e.Error.Message}");
        throw new Exception($"{nameof(Category)} SqlTableDependence error: {e.Error.Message}");
    }
}