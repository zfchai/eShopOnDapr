using Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.Consts;

var builder = WebApplication.CreateBuilder(args);

builder.ApplyAppsettings(args);
builder.AddCustomConfiguration();
builder.AddCustomSerilog();
builder.AddCustomSwagger();
builder.AddCustomAuthentication();
builder.AddCustomAuthorization();
builder.AddCustomDatabase();
builder.AddCustomHealthChecks();
builder.AddCustomApplicationServices();

builder.Services.AddDaprClient();
builder.Services.AddControllers();
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<OrderingProcessActor>();
});
builder.Services.AddSignalR();

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
app.MapActorsHandlers();
app.MapSubscribeHandler();
app.MapCustomHealthChecks("/hc", "/liveness", UIResponseWriter.WriteHealthCheckUIResponse);
app.MapHub<NotificationsHub>("/hub/notificationhub",
    options => options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling);

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



