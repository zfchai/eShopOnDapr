// Only use in this file to avoid conflicts with Microsoft.Extensions.Logging
using Serilog;
using Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.Consts;

namespace Microsoft.eShopOnDapr.Services.Ordering.API.Extensions;

public static class ProgramExtensions
{
    private const string DbConnString = "ConnectionStrings:OrderingDB"; 
    private const string SecretStore = "eshopondapr-secretstore"; 

    public static void ApplyAppsettings(this WebApplicationBuilder builder, string[] args)
    {
        // Retrieve the environmental information of the current application
        var appRoot = builder.Environment.ContentRootPath;
        var envName = builder.Environment.EnvironmentName;

        // Build the complete path of AppData/Settings/appsetings.json
        var customDefaultConfigPath = Path.Combine(appRoot, "AppData", "Settings", "appsettings.json");
        var customEnvConfigPath = Path.Combine(appRoot, "AppData", "Settings", $"appsettings.{envName}.json");

        // Add custom configuration file
        builder.Configuration.AddJsonFile(customDefaultConfigPath, optional: true, reloadOnChange: true)
                             .AddJsonFile(customEnvConfigPath, optional: true, reloadOnChange: true);

        // Add environment variables and command-line parameters
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.AddCommandLine(args);
    }

    public static void AddCustomConfiguration(this WebApplicationBuilder builder)
    {
        var daprClient = new DaprClientBuilder().Build();
        builder.Configuration.AddDaprSecretStore(SecretStore, daprClient);
    }

    public static void AddCustomOptions(this WebApplicationBuilder builder)
    {
        // Add custom extension configuration

    }

    public static void AddCustomSerilog(this WebApplicationBuilder builder)
    {
        var seqServerUrl = builder.Configuration["SeqServerUrl"];
        if (string.IsNullOrWhiteSpace(seqServerUrl))
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console()
            .Enrich.WithProperty("ApplicationName", Default.AppName)
            .CreateLogger();
        }
        else
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console()
            .WriteTo.Seq(seqServerUrl)
            .Enrich.WithProperty("ApplicationName", Default.AppName)
            .CreateLogger();
        }

        builder.Host.UseSerilog();
    }

    public static void AddCustomSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = $"eShopOnDapr - {Default.AppName}", Version = "v1" });

            var identityUrlExternal = builder.Configuration.GetValue<string>("IdentityUrlExternal");

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    Implicit = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri($"{identityUrlExternal}/connect/authorize"),
                        TokenUrl = new Uri($"{identityUrlExternal}/connect/token"),
                        Scopes = new Dictionary<string, string>()
                            {
                                { "ordering", Default.AppName }
                            }
                    }
                }
            });

            c.OperationFilter<AuthorizeCheckOperationFilter>();
        });
    }

    public static void UseCustomSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Default.AppName} V1");
            c.OAuthClientId("orderingswaggerui");
            c.OAuthAppName("Ordering Swagger UI");
        });
    }

    public static void AddCustomAuthentication(this WebApplicationBuilder builder)
    {
        // Prevent mapping "sub" claim to nameidentifier.
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.Audience = "ordering-api";
                options.Authority = builder.Configuration.GetValue<string>("IdentityUrl");
                options.RequireHttpsMetadata = false;
            });
    }

    public static void AddCustomAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "ordering");
            });
        });
    }

    public static void AddCustomApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventBus, DaprEventBus>();
        builder.Services.AddHttpContextAccessor();
        //builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IIdentityService, IdentityService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        builder.Services.AddScoped<IOrdersService, OrdersService>();
        builder.Services.AddScoped<IUpdateOrderStatusEventService, UpdateOrderStatusEventService>();
        builder.Services.AddScoped<IOrderingProcessEventService, OrderingProcessEventService>();
    }

    public static void AddCustomDatabase(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration[DbConnString];
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connString);
        builder.Services.AddDbContext<OrderingDbContext>(options => options.UseNpgsql(connString));
    }

    public static void ApplyDatabaseMigration(this WebApplication app)
    {
        // Apply database migration automatically. Note that this approach is not
        // recommended for production scenarios. Consider generating SQL scripts from
        // migrations instead.
        using var scope = app.Services.CreateScope();

        var retryPolicy = CreateRetryPolicy(app.Configuration, Log.Logger);
        var context = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();

        retryPolicy.Execute(context.Database.Migrate);
    }

    private static Policy CreateRetryPolicy(IConfiguration configuration, Serilog.ILogger logger)
    {
        // Only use a retry policy if configured to do so.
        // When running in an orchestrator/K8s, it will take care of restarting failed services.
        if (bool.TryParse(configuration["RetryMigrations"], out bool _))
        {
            return Policy.Handle<Exception>().
                WaitAndRetryForever(
                    sleepDurationProvider: _ => TimeSpan.FromSeconds(5),
                    onRetry: (exception, retry, _) =>
                    {
                        logger.Warning(
                            exception,
                            "Exception {ExceptionType} with message {Message} detected during database migration (retry attempt {retry})",
                            exception.GetType().Name,
                            exception.Message,
                            retry);
                    }
                );
        }

        return Policy.NoOp();
    }

    public static void AddCustomHealthChecks(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration[DbConnString];
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connString);
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddDapr()
            .AddNpgSql(
                connectionString: connString,
                name: "OrderingDB-check",
                tags: ["orderdb"]);
    }

    #region CustomOptions
    internal static void AddCustomOptions<TOptions>(this WebApplicationBuilder builder, string propertyName)
        where TOptions : class
    {
        var config = builder.Configuration;
        var services = builder.Services;
        var section = config.GetSection(propertyName);
        services.AddOptions<TOptions>(section);
    }

    internal static void AddOptions<TOptions>(this IServiceCollection services, IConfigurationSection section)
        where TOptions : class
    {
        services.AddOptions<TOptions>()
           .Bind(section, opt => opt.BindNonPublicProperties = true)
           .ValidateDataAnnotations();
    }
    #endregion
}
