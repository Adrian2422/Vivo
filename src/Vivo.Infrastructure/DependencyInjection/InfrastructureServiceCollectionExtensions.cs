using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Vivo.Infrastructure.Persistence.AppDbContext;

namespace Vivo.Infrastructure.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces;
using Repositories;

public static class InfrastructureServiceCollectionExtensions
{
    public static IHostApplicationBuilder AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("vivo-db")
                                 ?? throw new InvalidOperationException("Connection string 'MvcMovieContext' not found.")));
        
        builder.Services.AddScoped<IShortenedUrlRepository, ShortenedUrlRepository>();

        return builder;
    }
}