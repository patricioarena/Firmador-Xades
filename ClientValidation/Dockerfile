FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Configuración para producción
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["ClientValidation.csproj", "./"]
RUN dotnet restore "ClientValidation.csproj"

COPY . .
WORKDIR "/src/"
RUN dotnet build "./ClientValidation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ClientValidation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
# Copiar los archivos publicados
COPY --from=publish /app/publish .
# Asegurar que el directorio Resources existe y tiene los permisos correctos
RUN mkdir -p /app/Resources && \
    chmod -R 755 /app/Resources
    
ENTRYPOINT ["dotnet", "ClientValidation.dll"]