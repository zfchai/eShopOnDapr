namespace Microsoft.eShopOnDapr.Services.Catalog.API.ViewModel.Response;

public record PaginatedItemsResp(
    int PageIndex,
    int PageSize,
    long Count,
    IEnumerable<CatalogItemResp> Items);
