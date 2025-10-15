# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["EmployeeManager.API/EmployeeManager.API.csproj", "EmployeeManager.API/"]
COPY ["EmployeeManager.Application/EmployeeManager.Application.csproj", "EmployeeManager.Application/"]
COPY ["EmployeeManager.Infrastructure/EmployeeManager.Infrastructure.csproj", "EmployeeManager.Infrastructure/"]
COPY ["EmployeeManager.Domain/EmployeeManager.Domain.csproj", "EmployeeManager.Domain/"]
RUN dotnet restore "./EmployeeManager.API/EmployeeManager.API.csproj"
COPY . .
WORKDIR "/src/EmployeeManager.API"
RUN dotnet build "./EmployeeManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EmployeeManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeManager.API.dll"]