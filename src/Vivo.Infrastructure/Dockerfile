FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

RUN dotnet tool install --global dotnet-ef --version 10.0.*
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY ["src/Vivo.Domain/Vivo.Domain.csproj", "src/Vivo.Domain/"]
COPY ["src/Vivo.Application/Vivo.Application.csproj", "src/Vivo.Application/"]
COPY ["src/Vivo.Infrastructure/Vivo.Infrastructure.csproj", "src/Vivo.Infrastructure/"]
COPY ["src/Vivo.ServiceDefaults/Vivo.ServiceDefaults.csproj", "src/Vivo.ServiceDefaults/"]
COPY ["src/Vivo.ApiService/Vivo.ApiService.csproj", "src/Vivo.ApiService/"]

RUN dotnet restore "src/Vivo.ApiService/Vivo.ApiService.csproj"

COPY . .

RUN dotnet build "src/Vivo.ApiService/Vivo.ApiService.csproj" -c Release --no-restore

RUN dotnet ef migrations bundle \
    --project "src/Vivo.Infrastructure/Vivo.Infrastructure.csproj" \
    --startup-project "src/Vivo.ApiService/Vivo.ApiService.csproj" \
    --context "ApplicationDbContext" \
    --configuration Release \
    --output /app/efbundle \
    --self-contained \
    --no-build

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-noble AS final
WORKDIR /app
COPY --from=build /app/efbundle .

ENTRYPOINT ["/app/efbundle"]