using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlTableDependency.ServiceBroken.SubscribeTableDependency;

namespace SqlTableDependency.ServiceBroken.Configuration;
public static class ConfigureSqlServiceBroken
{
    public static void UseTableDependency(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
    {
        var scope = applicationBuilder.ApplicationServices.CreateScope();

        var subscribeProductTable = scope.ServiceProvider.GetRequiredService<SubscribeProductTableDependency>();
        subscribeProductTable.SubscribeProductDependency(configuration);

        var subscribeCategoryTable = scope.ServiceProvider.GetRequiredService<SubscribeCategoryTableDependency>();
        subscribeCategoryTable.SubscribeCategoryDependency(configuration);
    }
}
