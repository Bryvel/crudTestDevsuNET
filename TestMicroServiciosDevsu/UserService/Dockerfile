# --- Etapa 1: build (usa el SDK completo, pesado, solo para compilar) ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos solo el .csproj primero para aprovechar el cache de capas de Docker:
# si no cambian las dependencias, "dotnet restore" no se vuelve a ejecutar
# aunque cambies código fuente después.
COPY ["UserService.csproj", "./"]
RUN dotnet restore "UserService.csproj"

# Ahora sí copiamos el resto del código fuente
COPY . .
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Etapa 2: runtime (imagen final, liviana, sin el SDK) ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "UserService.dll"]
