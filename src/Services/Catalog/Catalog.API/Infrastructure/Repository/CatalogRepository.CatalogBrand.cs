using Microsoft.eShopOnDapr.Services.API.Abstraction.Paginated;
using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;
using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Mappers;
using Microsoft.eShopOnDapr.Services.Catalog.API.ViewModel.Response;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Repository;

public partial class CatalogRepository
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
