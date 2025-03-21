# Base image for runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Development Stage with SDK (Debug mode, for hot reload)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-dev
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY iftar-api/iftar-api.csproj iftar-api/
RUN dotnet restore "iftar-api/iftar-api.csproj"
COPY . .
WORKDIR "/src/iftar-api"
RUN dotnet build "iftar-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Production Stage (Release build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-prod
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["iftar-api/iftar-api.csproj", "iftar-api/"]
RUN dotnet restore "iftar-api/iftar-api.csproj"
COPY . .
WORKDIR "/src/iftar-api"
RUN dotnet publish "iftar-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage for production or development
FROM base AS final
WORKDIR /app
# Copy from the production build if in production mode or from dev if in development mode
COPY --from=build-prod /app/publish . 
# If you want dev build instead, replace `build-prod` with `build-dev`
ENTRYPOINT ["dotnet", "iftar-api.dll"]
