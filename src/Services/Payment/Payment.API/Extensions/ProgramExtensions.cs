// Only use in this file to avoid conflicts with Microsoft.Extensions.Logging
using Serilog;

namespace Microsoft.eShopOnDapr.Services.Payment.API.Extensions;

public static class ProgramExtensions
{
    private const string AppName = "Payment API";

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

    public static void AddCustomSerilog(this WebApplicationBuilder builder)
    {
        var seqServerUrl = builder.Configuration["SeqServerUrl"];

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console()
            .WriteTo.Seq(seqServerUrl!)
            .Enrich.WithProperty("ApplicationName", AppName)
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    public static void AddCustomSwagger(this WebApplicationBuilder builder) =>
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = $"eShopOnDapr - {AppName}", Version = "v1" });
        });

    public static void UseCustomSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppName} V1");
        });
    }

    public static void AddCustomHealthChecks(this WebApplicationBuilder builder) =>
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddDapr();

    public static void AddCustomApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<PaymentSetting>(builder.Configuration);

        builder.Services.AddScoped<IEventBus, DaprEventBus>();
        builder.Services.AddScoped<OrderStatusChangedToValidatedIntegrationEventHandler>();
    }
}
