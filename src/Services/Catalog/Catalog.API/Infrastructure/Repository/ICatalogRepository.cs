using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Repository;

public interface ICatalogRepository
{
    #region CatalogBrand
    IAsyncEnumerable<CatalogBrand> GetCatalogBrandsAsync();
    #endregion

    #region CatalogType
    IAsyncEnumerable<CatalogType> GetCatalogTypesAsync();
    #endregion

    #region CatalogItem
    IAsyncEnumerable<CatalogItem> GetCatalogItemsAsync();
    IAsyncEnumerable<CatalogItem?> GetCatalogItemsAsync(IEnumerable<string> ids);
    Task<PaginatedItems<CatalogItem>?> GetPaginatedCatalogItemsAsync(
        PaginatedFilter filter,
        string typeId,
        string brandId);
    #endregion

}
