using Serilog;

var appName = "Blazor UI Host";
var currentDirectory = Directory.GetCurrentDirectory();
var configuration = GetConfiguration(currentDirectory);

try
{
    Log.Logger = CreateSerilogLogger(configuration);

    Log.Information("Configuring web host ({ApplicationName})...", appName);
    var host = BuildWebHost();

    Log.Information("Starting web host ({ApplicationName})...", appName);
    await host.RunAsync();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationName})!", appName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

IConfiguration GetConfiguration(string basePath)
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

IHost BuildWebHost() =>
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
            webBuilder.UseContentRoot(currentDirectory);
        })
        .UseSerilog()
        .Build();

Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    var seqServerUrl = configuration["SeqServerUrl"];

    if (string.IsNullOrWhiteSpace(seqServerUrl))
    {
        return new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .WriteTo.Console()
        .Enrich.WithProperty("ApplicationName", appName)
        .CreateLogger();
    }

    return new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .WriteTo.Console()
        .WriteTo.Seq(seqServerUrl)
        .Enrich.WithProperty("ApplicationName", appName)
        .CreateLogger();
}
