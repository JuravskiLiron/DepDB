# ============================
# 1. BUILD STAGE
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj отдельно чтобы кешировать restore
COPY *.csproj ./
RUN dotnet restore

# Копируем весь проект
COPY . ./
RUN dotnet publish -c Release -o /app/publish


# ============================
# 2. RUNTIME STAGE
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Запускаем приложение
ENTRYPOINT ["dotnet", "DepDB.dll"]
