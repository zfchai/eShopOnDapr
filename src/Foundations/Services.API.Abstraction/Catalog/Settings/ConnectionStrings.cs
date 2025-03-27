
namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Catalog.Settings;

public sealed class ConnectionStrings : DatabaseBasicSettings
{
    public string CatalogDB { get; set; } = null!;
}
