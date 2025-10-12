
namespace Microsoft.eShopOnDapr.BuildingBlocks.Healthchecks.Options;

public sealed class HealthCheckUrl
{
    public string UriString { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public IEnumerable<string> Tags { get; set; } = [];

    public Uri GetUri() 
    {
        return new Uri(UriString);
    }
}
