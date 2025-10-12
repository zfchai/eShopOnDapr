namespace Microsoft.eShopOnDapr.BuildingBlocks.Healthchecks.Extensions;

public static class DaprHealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddDapr(this IHealthChecksBuilder builder) =>
        builder.AddCheck<DaprHealthCheck>("dapr");
}
