using Vivo.ApiService.Extensions;
using Vivo.Application.DependencyInjection;
using Vivo.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

builder.AddInfrastructureServices();
builder.Services.AddApplicationServices();

builder.Services.AddWebCors(builder.Environment, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await app.SeedDatabaseAsync();

    app.MapOpenApi();
}

app.UseWebCors();

app.UseExceptionHandler();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
