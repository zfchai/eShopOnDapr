
using Serilog;

namespace Microsoft.eShopOnDapr.Services.Identity.API.Extensions;

public static class ProgramExtensions
{
    private const string AppName = "Identity API";
    private const string DbConnString = "ConnectionStrings:IdentityDB";

    public static void AddCustomConfiguration(this WebApplicationBuilder builder)
    {
        var daprClient = new DaprClientBuilder().Build();
        builder.Configuration.AddDaprSecretStore("eshopondapr-secretstore", daprClient);
    }

    public static void AddCustomOptions(this WebApplicationBuilder builder)
    {
        // Add custom extension configuration

    }

    public static void AddCustomSerilog(this WebApplicationBuilder builder)
    {
        var seqServerUrl = builder.Configuration["SeqServerUrl"];

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console()
            .WriteTo.Seq(seqServerUrl)
            .Enrich.WithProperty("ApplicationName", AppName)
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    public static void AddCustomMvc(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();
    }

    public static void AddCustomDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration[DbConnString];
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
    }

    public static void AddCustomIdentity(this WebApplicationBuilder builder) =>
        builder.Services
           .AddIdentity<ApplicationUser, IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

    public static void AddCustomIdentityServer(this WebApplicationBuilder builder)
    {
        var identityServerBuilder = builder.Services.AddIdentityServer(options =>
        {
            options.IssuerUri = builder.Configuration["IssuerUrl"];
            options.Authentication.CookieLifetime = TimeSpan.FromHours(2);

            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
        })
        .AddInMemoryIdentityResources(Config.IdentityResources)
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryApiResources(Config.ApiResources)
        .AddInMemoryClients(Config.GetClients(builder.Configuration))
        .AddAspNetIdentity<ApplicationUser>();

        // not recommended for production - you need to store your key material somewhere secure
        identityServerBuilder.AddDeveloperSigningCredential();
    }

    public static void AddCustomAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
    }

    public static void AddCustomHealthChecks(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration[DbConnString];
        builder.Services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddNpgSql(connectionString, name: "IdentityDB-check", tags: ["IdentityDB"]);
    }

    public static void AddCustomApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IProfileService, ProfileService>();
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
