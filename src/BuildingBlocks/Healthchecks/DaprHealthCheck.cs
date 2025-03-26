namespace Microsoft.eShopOnDapr.BuildingBlocks.Healthchecks;

public class DaprHealthCheck(DaprClient daprClient) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken ct = default)
    {
        var healthy = await daprClient.CheckHealthAsync(ct);
        if (healthy)
        {
            return HealthCheckResult.Healthy("Dapr sidecar is healthy.");
        }

        return new HealthCheckResult(
            context.Registration.FailureStatus,
            "Dapr sidecar is unhealthy.");
    }
}
