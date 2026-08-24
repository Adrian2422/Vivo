namespace Vivo.Application.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Interfaces;
using Services;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IShortenedUrlService, ShortenedUrlService>();

        return services;
    }
}