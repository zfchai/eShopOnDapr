using Microsoft.eShopOnDapr.Services.API.Abstraction.Paginated;
using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;
using Microsoft.eShopOnDapr.Services.Catalog.API.ViewModel.Response;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Services;

public interface ICatalogService
{
    #region CatalogBrand
    IAsyncEnumerable<CatalogBrandResp> GetCatalogBrandsAsync();
    #endregion

    #region CatalogType
    IAsyncEnumerable<CatalogTypeResp> GetCatalogTypesAsync();
    #endregion

    #region CatalogItem
    IAsyncEnumerable<CatalogItemResp> GetCatalogItemsAsync();
    IAsyncEnumerable<CatalogItemResp?> GetCatalogItemsAsync(string ids);
    Task<PaginatedItems<CatalogItemResp>?> GetPaginatedCatalogItemsAsync(
        PaginatedFilter filter,
        string typeId,
        string brandId); 
    #endregion

}
