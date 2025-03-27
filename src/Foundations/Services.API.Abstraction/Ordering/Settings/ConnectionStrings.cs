namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.Settings;

public sealed class ConnectionStrings : DatabaseBasicSettings
{
    public string OrderingDB { get; set; } = null!;
}
