FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Runner/Runner.csproj", "Runner/"]
COPY ["CreditCards.Contracts/CreditCards.Contracts.csproj", "CreditCards.Contracts/"]
COPY ["CreditCards/CreditCards.csproj", "CreditCards/"]
COPY ["EpsAdapter.Contracts/EpsAdapter.Contracts.csproj", "EpsAdapter.Contracts/"]
COPY ["EpsAdapter/EpsAdapter.csproj", "EpsAdapter/"]
RUN dotnet restore "./Runner/./Runner.csproj"
COPY . .
WORKDIR "/src/Runner"
RUN dotnet build "./Runner.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Runner.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Runner.dll"]