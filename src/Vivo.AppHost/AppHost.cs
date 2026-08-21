var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Vivo_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

var web = builder.AddViteApp("web", "../Vivo.Web")
    .WithRunScript("start")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
