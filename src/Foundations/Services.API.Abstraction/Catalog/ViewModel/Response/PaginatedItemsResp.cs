namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Catalog.ViewModel.Response;

public record PaginatedItemsResp(
    int PageIndex,
    int PageSize,
    long Count,
    IEnumerable<CatalogItemResp> Items);
