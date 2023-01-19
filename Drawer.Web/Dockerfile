#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Drawer.Web/Drawer.Web.csproj", "Drawer.Web/"]
COPY ["Drawer.AidBlazor/Drawer.AidBlazor.csproj", "Drawer.AidBlazor/"]
COPY ["Drawer.Application/Drawer.Application.csproj", "Drawer.Application/"]
COPY ["Drawer.Domain/Drawer.Domain.csproj", "Drawer.Domain/"]
COPY ["Drawer.Shared/Drawer.Shared.csproj", "Drawer.Shared/"]
RUN dotnet restore "Drawer.Web/Drawer.Web.csproj"
COPY . .
WORKDIR "/src/Drawer.Web"
RUN dotnet build "Drawer.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Drawer.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Drawer.Web.dll"]