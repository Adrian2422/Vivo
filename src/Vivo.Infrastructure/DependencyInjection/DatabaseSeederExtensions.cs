using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vivo.Infrastructure.Persistence;

namespace Vivo.Infrastructure.DependencyInjection;

public static class DatabaseSeederExtensions
{
    public static async Task SeedDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

        await seeder.SeedAsync();
    }
}