using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Mappers;
using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Repository;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Services;

public class CatalogService(ICatalogRepository repository) : ICatalogService
{
    #region CatalogBrand
    public async IAsyncEnumerable<CatalogBrandResp> GetCatalogBrandsAsync()
    {
        var items = repository.GetCatalogBrandsAsync();
        await foreach (var item in items)
        {
            yield return item.To();
        }
    }
    #endregion

    #region CatalogType
    public async IAsyncEnumerable<CatalogTypeResp> GetCatalogTypesAsync()
    {
        var items = repository.GetCatalogTypesAsync();
        await foreach (var item in items)
        {
            yield return item.To();
        }
    } 
    #endregion

    public async IAsyncEnumerable<CatalogItemResp> GetCatalogItemsAsync()
    {
        var items = repository.GetCatalogItemsAsync();
        await foreach (var item in items)
        {
            yield return item.To();
        }
    }

    public async IAsyncEnumerable<CatalogItemResp?> GetCatalogItemsAsync(string ids) 
    {
        var numIds = ids.Split(',');
        var items = repository.GetCatalogItemsAsync(numIds);
        await foreach (var item in items)
        {
            yield return item?.To();
        }
    }

    public async Task<PaginatedItems<CatalogItemResp>?> GetPaginatedCatalogItemsAsync(
        PaginatedFilter filter,
        string typeId,
        string brandId)
    {
        List<CatalogItemResp> list = [];
        var paginatedItems = await repository.GetPaginatedCatalogItemsAsync(filter, typeId, brandId);
        long count = paginatedItems?.Count ?? 0;
        var items = paginatedItems?.Items;

        if (items != null)
        {
            foreach (var item in items)
            {
                list.Add(item.To());
            }
        }

        return new PaginatedItems<CatalogItemResp>(filter, count, list);
    }


}
