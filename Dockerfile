# Étape 1 : Image de base pour l'exécution
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Étape 2 : Configuration du développement avec SDK pour le Hot Reload
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-dev
WORKDIR /src
COPY iftar-api/iftar-api.csproj ./iftar-api/
RUN dotnet restore ./iftar-api/iftar-api.csproj
COPY . /src/
WORKDIR /src/iftar-api
RUN dotnet build ./iftar-api.csproj -c Debug -o /app/build

# Étape 3 : Étape de production (Release)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-prod
WORKDIR /src
COPY iftar-api/iftar-api.csproj ./iftar-api/
RUN dotnet restore ./iftar-api/iftar-api.csproj
COPY . /src/
WORKDIR /src/iftar-api
RUN dotnet publish ./iftar-api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Étape 4 : Production, copie du code compilé et exécution
FROM base AS final
WORKDIR /app
COPY --from=build-prod /app/publish .
# Installer dotnet-ef dans l'image de production (si nécessaire pour gérer la base de données en prod)
RUN dotnet tool install --global dotnet-ef
# Ajout du répertoire des outils globalement installés à PATH
ENV PATH="${PATH}:/root/.dotnet/tools"
CMD ["dotnet", "iftar-api.dll"]

# Étape 5 : Mode Développement avec Hot Reload
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev
WORKDIR /src/iftar-api
COPY --from=build-dev /app/build .
COPY . /src/
# Installer dotnet-ef dans l'image de développement
RUN dotnet tool install --global dotnet-ef
# Ajout du répertoire des outils globalement installés à PATH
ENV PATH="${PATH}:/root/.dotnet/tools"
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
CMD ["dotnet", "watch", "run", "--no-launch-profile", "--urls", "http://0.0.0.0:8080", "--project", "/src/iftar-api/iftar-api.csproj"]
