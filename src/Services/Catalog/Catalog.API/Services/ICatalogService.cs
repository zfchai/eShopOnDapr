namespace Microsoft.eShopOnDapr.Services.Catalog.API.Services;

public interface ICatalogService
{
    Task<List<CatalogBrand>> GetCatalogBrandsAsync();
    Task<List<CatalogType>> GetCatalogTypesAsync();
    Task<List<ItemViewModel>> GetCatalogItemsAsync(string ids);
    Task<PaginatedItemsViewModel> GetPaginatedCatalogItemsAsync(
        int typeId = -1,
        int brandId = -1,
        int pageSize = 10,
        int pageIndex = 0);



}
