using Microsoft.eShopOnDapr.Services.API.Abstraction.Database;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Identity.Settings;

public sealed class ConnectionString : DatabaseBasicSetting
{
    public string IdentityDB { get; set; } = null!;
}
