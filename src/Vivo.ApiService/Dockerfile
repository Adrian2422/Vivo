FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Vivo.ApiService/Vivo.ApiService.csproj", "src/Vivo.ApiService/"]
COPY ["src/Vivo.ServiceDefaults/Vivo.ServiceDefaults.csproj", "src/Vivo.ServiceDefaults/"]
COPY ["src/Vivo.Infrastructure/Vivo.Infrastructure.csproj", "src/Vivo.Infrastructure/"]
COPY ["src/Vivo.Application/Vivo.Application.csproj", "src/Vivo.Application/"]
COPY ["src/Vivo.Domain/Vivo.Domain.csproj", "src/Vivo.Domain/"]

RUN dotnet restore "src/Vivo.ApiService/Vivo.ApiService.csproj"
COPY . .
WORKDIR "/src/src/Vivo.ApiService"
RUN dotnet build "Vivo.ApiService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Vivo.ApiService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vivo.ApiService.dll"]