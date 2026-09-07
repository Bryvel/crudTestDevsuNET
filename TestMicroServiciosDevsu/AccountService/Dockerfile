# --- Etapa 1: build ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["AccountService.csproj", "./"]
RUN dotnet restore "AccountService.csproj"

COPY . .
RUN dotnet publish "AccountService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Etapa 2: runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AccountService.dll"]
