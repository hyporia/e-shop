using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shared.Data.Migrator;

public class DbMigrator<TContext>(
    ILogger<DbMigrator<TContext>> logger,
    IServiceScopeFactory serviceScopeFactory,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    where TContext : DbContext
{
    public static readonly string ActivitySourceName = $"{typeof(TContext).Name}_Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await CreateOrMigrateDatabaseAsync(stoppingToken);
        hostApplicationLifetime.StopApplication();
    }

    private async Task CreateOrMigrateDatabaseAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            await using var scope = serviceScopeFactory.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

            await RunMigrationAsync(dbContext, cancellationToken);

            var dataSeeder = scope.ServiceProvider.GetService<IDataSeeder<TContext>>();
            if (dataSeeder is not null && dataSeeder.ShouldSeed(dbContext))
            {
                await SeedDataAsync(dbContext, dataSeeder, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Cannot migrate the database");
            activity?.AddException(ex);
            Environment.ExitCode = 1;
        }
    }

    private static async Task RunMigrationAsync(TContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await dbContext.Database.MigrateAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(TContext dbContext, IDataSeeder<TContext> dataSeeder, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.AddRangeAsync(dataSeeder.Entities, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}