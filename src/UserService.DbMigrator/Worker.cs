using Microsoft.EntityFrameworkCore;
using UserService.Data;

namespace UserService.DbMigrator;

public class Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory, IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await CreateOrMigrateDatabaseAsync(stoppingToken);
		hostApplicationLifetime.StopApplication();
	}

	private async Task CreateOrMigrateDatabaseAsync(CancellationToken stoppingToken)
	{
		await using var scope = serviceScopeFactory.CreateAsyncScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

		var databaseWasCreated = await dbContext.Database.EnsureCreatedAsync(stoppingToken);
		if (databaseWasCreated)
		{
			logger.LogInformation("Database was created");
			return;
		}

		var pendingMigrationsExist = dbContext.Database.HasPendingModelChanges();
		if (pendingMigrationsExist)
		{
			logger.LogInformation("Applying pending migrations");
			await dbContext.Database.MigrateAsync(stoppingToken);
			return;
		}

		logger.LogInformation("No pending migrations");
	}
}
