namespace Vivo.ApiService.Extensions;

public static class CorsExtensions
{
    private const string PolicyName = "WebClient";

    public static IServiceCollection AddWebCors(
        this IServiceCollection services,
        IHostEnvironment env,
        IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                if (env.IsDevelopment())
                {
                    policy
                        .SetIsOriginAllowed(origin =>
                        {
                            var uri = new Uri(origin);
                            return uri.Host == "localhost";
                        })
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
                else
                {
                    var origins = configuration
                        .GetSection("Cors:AllowedOrigins")
                        .Get<string[]>() ?? [];
                    policy
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseWebCors(
        this IApplicationBuilder app)
    {
        app.UseCors(PolicyName);

        return app;
    }
}