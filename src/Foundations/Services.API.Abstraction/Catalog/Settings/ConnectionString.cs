
using Microsoft.eShopOnDapr.Services.API.Abstraction.Database;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Catalog.Settings;

public sealed class ConnectionString : DatabaseBasicSetting
{
    public string CatalogDB { get; set; } = null!;
}
