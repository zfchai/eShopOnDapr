using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Repository;

internal partial class CatalogRepository
{
    #region CatalogType
    public async IAsyncEnumerable<CatalogType> GetCatalogTypesAsync()
    {
        var entities = await _context.CatalogTypes.ToListAsync();
        foreach (var item in entities)
        {
            yield return item;
        }
    }

    #endregion
}
