namespace Microsoft.eShopOnDapr.Services.Identity.API.Infrastructure.Data;

internal class SeedData
{
    public static async Task EnsureSeedDataAsync(IServiceScope scope, IConfiguration configuration, ILogger logger)
    {
        var retryPolicy = CreateRetryPolicy(configuration, logger);
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await retryPolicy.ExecuteAsync(async () =>
        {
            await context.Database.MigrateAsync();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // create alice
            await CreateUserAsync(userMgr, logger, "Pass123$", new ApplicationUser
            {
                UserName = "alice",
                Email = "AliceSmith@email.com",
                EmailConfirmed = true,
                CardHolderName = "Alice Smith",
                CardNumber = "4012888888881881",
                CardType = 1,
                City = "Redmond",
                Country = "U.S.",
                Expiration = "12/20",
                Id = Guid.NewGuid().ToString(),
                LastName = "Smith",
                Name = "Alice",
                PhoneNumber = "1234567890",
                ZipCode = "98052",
                State = "WA",
                Street = "15703 NE 61st Ct",
                SecurityNumber = "123"
            });

            // create bob
            await CreateUserAsync(userMgr, logger, "Pass123$", new ApplicationUser
            {
                UserName = "bob",
                Email = "BobSmith@email.com",
                EmailConfirmed = true,
                CardHolderName = "Bob Smith",
                CardNumber = "4012888888881881",
                CardType = 1,
                City = "Redmond",
                Country = "U.S.",
                Expiration = "12/20",
                Id = Guid.NewGuid().ToString(),
                LastName = "Smith",
                Name = "Bob",
                PhoneNumber = "1234567890",
                ZipCode = "98052",
                State = "WA",
                Street = "15703 NE 61st Ct",
                SecurityNumber = "456"
            });
        });
    }

    private static async Task CreateUserAsync(UserManager<ApplicationUser> userMgr, ILogger logger, string password, ApplicationUser appUser)
    {
        var user = await userMgr.FindByNameAsync(appUser.UserName);
        if (user == null)
        {
            var result = await userMgr.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            logger.LogDebug($"{appUser.UserName} created");
        }
        else
        {
            logger.LogDebug($"{appUser.UserName} already exists");
        }
    }


    private static AsyncPolicy CreateRetryPolicy(IConfiguration configuration, ILogger logger)
    {
        bool.TryParse(configuration["RetryMigrations"], out bool retryMigrations);

        // Only use a retry policy if configured to do so.
        // When running in an orchestrator/K8s, it will take care of restarting failed services.
        if (retryMigrations)
        {
            return Policy.Handle<Exception>().
                WaitAndRetryForeverAsync(
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, retry, timeSpan) =>
                    {
                        logger.LogWarning(
                            exception,
                            "Exception {ExceptionType} with message {Message} detected during database migration (retry attempt {retry})",
                            exception.GetType().Name,
                            exception.Message,
                            retry);
                    }
                );
        }

        return Policy.NoOpAsync();
    }
}
