var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHealthChecksUI()
    .AddInMemoryStorage();

builder.Logging.AddJsonConsole();

var app = builder.Build();

var pathBase = builder.Configuration["PathBase"];
if (!string.IsNullOrWhiteSpace(pathBase))
{
    app.UsePathBase(pathBase);
}

app.UseHealthChecksUI(config =>
{
    config.ResourcesPath = string.IsNullOrWhiteSpace(pathBase)
        ? "/ui/resources"
        : $"{pathBase}/ui/resources";
});

app.MapGet(string.IsNullOrWhiteSpace(pathBase)
    ? "/"
    : pathBase, () => Results.LocalRedirect("~/healthchecks-ui"));

app.MapHealthChecksUI();

await app.RunAsync();
