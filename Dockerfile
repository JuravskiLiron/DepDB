# --- BUILD STAGE ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everything and build
COPY . .
RUN dotnet publish -c Release -o /app/publish

# --- RUNTIME STAGE ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app
COPY --from=build /app/publish .

# Environment variables for Render
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "DepDB.dll"]
### # --- BUILD STAGE ---
    #FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
    #WORKDIR /src
    #
    ## копируем .csproj
    #COPY *.csproj ./
    #
    ## restore
    #RUN dotnet restore
    #
    ## копируем весь проект
    #COPY . .
    #
    ## publish
    #RUN dotnet publish -c Release -o /app/publish
    #
    ## --- RUNTIME STAGE ---
    #FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
    #WORKDIR /app
    #
    #COPY --from=build /app/publish .
    #
    #ENV ASPNETCORE_URLS=http://+:10000
    #EXPOSE 10000
    #
    #ENTRYPOINT ["dotnet", "DepDB.dll"]
