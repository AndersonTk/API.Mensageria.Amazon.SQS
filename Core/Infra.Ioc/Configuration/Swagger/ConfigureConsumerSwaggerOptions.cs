using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infra.Ioc.Configuration.Swagger;

public class ConfigureConsumerSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    readonly IApiVersionDescriptionProvider provider;

    public ConfigureConsumerSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;



    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "Api - Catalogo de produtos - Consumer ",
            Version = description.ApiVersion.ToString(),
            Description = "API catalogo de produtos! essa api consume o catalogo de produtos.",
            Contact = new OpenApiContact() { Name = "DEV | FullStack|Asp .Net Core - Anderson Pinheiro", Email = "andersomlimapinheiro@gmail.com" },
            TermsOfService = new Uri("https://opensource.org/licenses/MIT"),
            License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
        };

        if (description.IsDeprecated)
        {
            info.Description += "Está versão está obsoleta";
        }

        return info;
    }
}
