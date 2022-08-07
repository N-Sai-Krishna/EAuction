#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["EAuction.API/Seller.API/Seller.API.csproj", "EAuction.API/Seller.API/"]
COPY ["EAuction.Core/EAuction.Core.Common/EAuction.Core.Common.csproj", "EAuction.Core/EAuction.Core.Common/"]
COPY ["EAuction.Core/Seller.Core/Seller.Core.csproj", "EAuction.Core/Seller.Core/"]
COPY ["EAuction.Infrastructure/EAuction.Persistence/EAuction.Persistence.csproj", "EAuction.Infrastructure/EAuction.Persistence/"]
COPY ["EAuction.Infrastructure/EAuction.Common/EAuction.Common.csproj", "EAuction.Infrastructure/EAuction.Common/"]
COPY ["EAuction.Infrastructure/EAuction.Messaging/EAuction.Messaging.csproj", "EAuction.Infrastructure/EAuction.Messaging/"]
RUN dotnet restore "EAuction.API/Seller.API/Seller.API.csproj"
COPY . .
WORKDIR "/src/EAuction.API/Seller.API"
RUN dotnet build "Seller.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Seller.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Seller.API.dll"]