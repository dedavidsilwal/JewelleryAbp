#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Jewellery.Web.Host/Jewellery.Web.Host.csproj", "src/Jewellery.Web.Host/"]
COPY ["src/Jewellery.Web.Core/Jewellery.Web.Core.csproj", "src/Jewellery.Web.Core/"]
COPY ["src/Jewellery.Application/Jewellery.Application.csproj", "src/Jewellery.Application/"]
COPY ["src/Jewellery.Core/Jewellery.Core.csproj", "src/Jewellery.Core/"]
COPY ["src/Jewellery.EntityFrameworkCore/Jewellery.EntityFrameworkCore.csproj", "src/Jewellery.EntityFrameworkCore/"]
RUN dotnet restore "src/Jewellery.Web.Host/Jewellery.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/Jewellery.Web.Host"
RUN dotnet build "Jewellery.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Jewellery.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jewellery.Web.Host.dll"]
