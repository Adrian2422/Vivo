namespace Vivo.Infrastructure.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.AppDbContext;
using Repositories;
using Application.Interfaces;

public static class InfrastructureServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("vivo-db")
                                 ?? throw new InvalidOperationException("Connection string 'MvcMovieContext' not found.")));
        
        builder.Services.AddScoped<IShortenedUrlRepository, ShortenedUrlRepository>();
        builder.Services.AddScoped<DatabaseSeeder>();
        return builder;
    }
}