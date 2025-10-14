using Microsoft.eShopOnDapr.Services.API.Abstraction.Catalog.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.ApplyAppsettings(args);
builder.AddCustomConfiguration();
builder.AddCustomOptions();
builder.AddCustomSerilog();
builder.AddCustomSwagger();
builder.AddCustomDatabase();
builder.AddCustomHealthChecks();
builder.AddCustomApplicationServices();

builder.Services.AddDaprClient();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseCustomSwagger();
}

var pathBase = builder.Configuration["PathBase"];
if (!string.IsNullOrWhiteSpace(pathBase))
{
    app.UsePathBase(pathBase);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Pics")),
    RequestPath = "/pics"
});

app.UseCloudEvents();

app.MapGet("/", () => Results.LocalRedirect("~/swagger"));
app.MapControllers();
app.MapSubscribeHandler();
app.MapCustomHealthChecks("/hc", "/liveness", UIResponseWriter.WriteHealthCheckUIResponse);

try
{
    app.Logger.LogInformation("Applying database migration ({ApplicationName})...", Default.AppName);
    app.ApplyDatabaseMigration();

    app.Logger.LogInformation("Starting web host ({ApplicationName})...", Default.AppName);
    await app.RunAsync();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "Host terminated unexpectedly ({ApplicationName})...", Default.AppName);
}
finally
{
    Serilog.Log.CloseAndFlush();
}
