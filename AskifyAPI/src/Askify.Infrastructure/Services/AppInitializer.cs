using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Askify.Infrastructure.Services;

internal class AppInitializer(IServiceProvider serviceProvider, ILogger<AppInitializer> logger)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<AskifyDbContext>();

        var pendingMigrations = await dbContext.Database
            .GetPendingMigrationsAsync(cancellationToken);

        if (CanMigrate(dbContext, pendingMigrations))
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
            logger.LogInformation("Migrated database to the latest version");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static bool CanMigrate(AskifyDbContext dbContext, IEnumerable<string> pendingMigrations)
        => dbContext.Database.IsRelational() && pendingMigrations.Any();
}