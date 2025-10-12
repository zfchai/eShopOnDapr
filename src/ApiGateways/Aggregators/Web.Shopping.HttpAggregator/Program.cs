using Microsoft.eShopOnDapr.Web.Shopping.HttpAggregator.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.ApplyAppsettings(args);
builder.AddCustomSerilog();
builder.AddCustomSwagger();
builder.AddCustomAuthentication();
builder.AddCustomAuthorization();
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

app.UseCloudEvents();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.LocalRedirect("~/swagger"));
app.MapControllers();
app.MapCustomHealthChecks("/hc", "/liveness", UIResponseWriter.WriteHealthCheckUIResponse);

try
{
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
