using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using UserService.Data;

namespace UserService.DbMigrator;

public class Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory, IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await CreateOrMigrateDatabaseAsync(stoppingToken);
		hostApplicationLifetime.StopApplication();
	}

	private async Task CreateOrMigrateDatabaseAsync(CancellationToken cancellationToken)
	{
		try
		{
			await using var scope = serviceScopeFactory.CreateAsyncScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

			await EnsureDatabaseAsync(dbContext, cancellationToken);
			await RunMigrationAsync(dbContext, cancellationToken);
		}
		catch (Exception ex)
		{
			logger.LogCritical(ex, "Cannot migrate the database");
			Environment.ExitCode = 1;
		}
	}

	private async Task EnsureDatabaseAsync(UserDbContext dbContext, CancellationToken cancellationToken)
	{
		var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

		var strategy = dbContext.Database.CreateExecutionStrategy();
		await strategy.ExecuteAsync(async () =>
		{
			// Create the database if it does not exist.
			// Do this first so there is then a database to start a transaction against.
			if (!await dbCreator.ExistsAsync(cancellationToken))
			{
				await dbCreator.CreateAsync(cancellationToken);
				logger.LogInformation("Database was created");
			}
		});
	}

	private async Task RunMigrationAsync(UserDbContext dbContext, CancellationToken cancellationToken)
	{
		var pendingMigrationsExist = dbContext.Database.HasPendingModelChanges();
		if (!pendingMigrationsExist)
		{
			logger.LogInformation("No pending migrations");
			return;
		}

		var strategy = dbContext.Database.CreateExecutionStrategy();
		await strategy.ExecuteAsync(async () =>
		{
			// Run migration in a transaction to avoid partial migration if it fails.
			await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
			await dbContext.Database.MigrateAsync(cancellationToken);
			await transaction.CommitAsync(cancellationToken);
		});

		logger.LogInformation("Pending migrations applied");
	}
}
