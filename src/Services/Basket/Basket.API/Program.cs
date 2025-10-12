using Microsoft.eShopOnDapr.Services.API.Abstraction.Basket.Consts;

var builder = WebApplication.CreateBuilder();

builder.ApplyAppsettings(args);
builder.AddCustomSerilog();
builder.AddCustomSwagger();
builder.AddCustomMvc();
builder.AddCustomAuthentication();
builder.AddCustomAuthorization();
builder.AddCustomHealthChecks();
builder.AddCustomApplicationServices();

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

app.UseCors("CorsPolicy");
app.MapDefaultControllerRoute();
app.MapControllers();
app.MapSubscribeHandler();
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
