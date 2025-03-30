using Microsoft.eShopOnDapr.Services.API.Abstraction.Catalog.ViewModel.Response;
using Microsoft.eShopOnDapr.Services.API.Abstraction.Paginated;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Catalog.Services;

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
