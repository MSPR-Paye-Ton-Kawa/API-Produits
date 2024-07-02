# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copier le fichier csproj et restaurer les dépendances
COPY ["API_Produits.csproj", "."]
RUN dotnet restore "API_Produits.csproj"

# Copier tous les fichiers et compiler
COPY . .
RUN dotnet build "API_Produits.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "API_Produits.csproj" -c Release -o /app/publish

# Stage 3: Final
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API_Produits.dll"]
