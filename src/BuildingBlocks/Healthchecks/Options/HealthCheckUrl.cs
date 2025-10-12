
namespace Microsoft.eShopOnDapr.BuildingBlocks.Healthchecks.Options;

public sealed class HealthCheckUrl
{
    public string UriString { get; set; }

    public string Name { get; set; }

    public IEnumerable<string> Tags { get; set; }

    public Uri GetUri() 
    {
        return new Uri(UriString);
    }
}
