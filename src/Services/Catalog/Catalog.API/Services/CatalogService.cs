namespace Microsoft.eShopOnDapr.Services.Catalog.API.Services;

public class CatalogService : ICatalogService
{
    private readonly CatalogDbContext _context;
   
    public CatalogService(CatalogDbContext context)
    {
        _context = context;
        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public Task<List<CatalogBrand>> GetCatalogBrandsAsync() =>
        _context.CatalogBrands.ToListAsync();

    public Task<List<CatalogType>> GetCatalogTypesAsync() =>
        _context.CatalogTypes.ToListAsync();

    public async Task<List<ItemViewModel>> GetCatalogItemsAsync(string ids) 
    {
        List<ItemViewModel> items = [];
        var numIds = ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x));
        if (numIds.All(nid => nid.Ok))
        {
            var idsToSelect = numIds.Select(id => id.Value);
            items = await _context.CatalogItems
                .Where(ci => idsToSelect.Contains(ci.Id))
                .Select(item => new ItemViewModel(
                   item.Id,
                   item.Name,
                   item.Price,
                   item.PictureFileName))
                .ToListAsync();
        }

        return items;
    }

    public async Task<PaginatedItemsViewModel> GetPaginatedCatalogItemsAsync(
        int typeId = -1,
        int brandId = -1,
        int pageSize = 10,
        int pageIndex = 0)
    {
        var query = _context.CatalogItems.AsQueryable();

        if (typeId > -1)
            query = query.Where(ci => ci.CatalogTypeId == typeId);

        if (brandId > -1)
            query = query.Where(ci => ci.CatalogBrandId == brandId);

        var totalItems = await query.LongCountAsync();

        var itemsOnPage = await query
            .OrderBy(item => item.Name)
            .Skip(pageSize * pageIndex)
            .Take(pageSize)
            .Select(item => new ItemViewModel(
               item.Id,
               item.Name,
               item.Price,
               item.PictureFileName))
            .ToListAsync();

        return new PaginatedItemsViewModel(pageIndex, pageSize, totalItems, itemsOnPage ?? []);
    }


}
