#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Drawer.Api/Drawer.Api.csproj", "Drawer.Api/"]
COPY ["Drawer.Application/Drawer.Application.csproj", "Drawer.Application/"]
COPY ["Drawer.Domain/Drawer.Domain.csproj", "Drawer.Domain/"]
COPY ["Drawer.Shared/Drawer.Shared.csproj", "Drawer.Shared/"]
COPY ["Drawer.Infrastructure/Drawer.Infrastructure.csproj", "Drawer.Infrastructure/"]
RUN dotnet restore "Drawer.Api/Drawer.Api.csproj"
COPY . .
WORKDIR "/src/Drawer.Api"
RUN dotnet build "Drawer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Drawer.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Drawer.Api.dll"]