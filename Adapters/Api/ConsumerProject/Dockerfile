# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Adapters/Api/ConsumerProject/ConsumerProject.csproj", "Adapters/Api/ConsumerProject/"]
COPY ["Core/Infra.Ioc/Infra.Ioc.csproj", "Core/Infra.Ioc/"]
COPY ["Adapters/Cache/Cache.csproj", "Adapters/Cache/"]
COPY ["Core/Domain/Domain.csproj", "Core/Domain/"]
COPY ["Core/Resources/Resources.csproj", "Core/Resources/"]
COPY ["Adapters/Infra.Data/Infra.Data.csproj", "Adapters/Infra.Data/"]
COPY ["Adapters/SqlTableDependency.ServiceBroken/SqlTableDependency.ServiceBroken.csproj", "Adapters/SqlTableDependency.ServiceBroken/"]
COPY ["Core/Application/Application.csproj", "Core/Application/"]
RUN dotnet restore "./Adapters/Api/ConsumerProject/ConsumerProject.csproj"
COPY . .
WORKDIR "/src/Adapters/Api/ConsumerProject"
RUN dotnet build "./ConsumerProject.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ConsumerProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsumerProject.dll"]