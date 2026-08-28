var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameter("sql-password", secret: true);

var db = builder
    .AddSqlServer("db", password: sqlPassword)
    .WithDataVolume("vivo-data")
    .WithLifetime(ContainerLifetime.Session)
    .AddDatabase("VivoDb");

var apiService = builder.AddProject<Projects.Vivo_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(db)
    .WithExternalHttpEndpoints()
    .WaitFor(db);

var web = builder.AddViteApp("web", "../Vivo.Web")
    .WithRunScript("start")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
