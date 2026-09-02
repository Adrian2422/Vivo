using Microsoft.OpenApi;
using Vivo.ApiService.Extensions;
using Vivo.Application.DependencyInjection;
using Vivo.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProblemDetails();

builder.AddInfrastructureServices();
builder.Services.AddApplicationServices();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "Vivo API", Version = "v1" });
        options.SupportNonNullableReferenceTypes();
        options.EnableAnnotations();
    });
}

builder.Services.AddWebCors(builder.Environment, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.MigrateAndSeedDatabaseAsync();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Vivo API V1");
    });
}

app.UseWebCors();

app.UseExceptionHandler();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();
