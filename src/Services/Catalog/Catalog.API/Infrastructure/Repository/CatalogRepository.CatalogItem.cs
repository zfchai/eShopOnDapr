using Microsoft.eShopOnDapr.Services.API.Abstraction.Paginated;
using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;
using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Mappers;
using Microsoft.eShopOnDapr.Services.Catalog.API.ViewModel.Response;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Repository;

public partial class CatalogRepository
{
    #region CatalogItem
    public async IAsyncEnumerable<CatalogItem> GetCatalogItemsAsync()
    {
        var entities = await _context.CatalogItems.ToListAsync();
        foreach (var item in entities)
        {
            yield return item;
        }
    }

    public async IAsyncEnumerable<CatalogItem?> GetCatalogItemsAsync(IEnumerable<string> ids)
    {
        ids = ids.Where(x => !string.IsNullOrWhiteSpace(x));
        var items = await _context.CatalogItems
                .Where(ci => ids.Contains(ci.Id))
                .ToListAsync();

        foreach (var item in items)
        {
            yield return item;
        }
    }

    public async Task<PaginatedItems<CatalogItem>?> GetPaginatedCatalogItemsAsync(
        PaginatedFilter filter,
        string typeId,
        string brandId)
    {
        var query = _context.CatalogItems.AsQueryable();

        if (!string.IsNullOrWhiteSpace(typeId))
            query = query.Where(ci => ci.CatalogTypeId == typeId);

        if (!string.IsNullOrWhiteSpace(brandId))
            query = query.Where(ci => ci.CatalogBrandId == brandId);

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query
            .OrderBy(item => item.Name)
            .Skip(filter.PageSize * filter.PageIndex)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PaginatedItems<CatalogItem>(filter, totalItems, itemsOnPage);
    }
    #endregion

}
