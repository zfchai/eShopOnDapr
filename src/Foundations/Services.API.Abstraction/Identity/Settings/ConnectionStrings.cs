namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Identity.Settings;

public sealed class ConnectionStrings : DatabaseBasicSettings
{
    public string IdentityDB { get; set; } = null!;
}
