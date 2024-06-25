#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsumerProject/ConsumerProject.csproj", "ConsumerProject/"]
COPY ["Infra.Ioc/Infra.Ioc.csproj", "Infra.Ioc/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Consumers/Domain.csproj", "Consumers/"]
COPY ["Resources/Resources.csproj", "Resources/"]
COPY ["Infra.Data/Infra.Data.csproj", "Infra.Data/"]
RUN dotnet restore "ConsumerProject/ConsumerProject.csproj"
COPY . .
WORKDIR "/src/ConsumerProject"
RUN dotnet build "ConsumerProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsumerProject.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConsumerProject.dll"]