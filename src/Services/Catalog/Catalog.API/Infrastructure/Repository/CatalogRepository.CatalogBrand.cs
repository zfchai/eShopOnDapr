using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Repository;

internal partial class CatalogRepository
{
    #region CatalogBrand
    public async IAsyncEnumerable<CatalogBrand> GetCatalogBrandsAsync() 
    {
        var entities = await _context.CatalogBrands.ToListAsync();
        foreach (var item in entities)
        {
            yield return item;
        }
    }
    
    #endregion
}
