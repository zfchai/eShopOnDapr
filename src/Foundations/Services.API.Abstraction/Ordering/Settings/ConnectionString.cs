namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.Settings;

public sealed class ConnectionString : DatabaseBasicSetting
{
    public string OrderingDB { get; set; } = null!;
}
