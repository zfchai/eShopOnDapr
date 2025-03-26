namespace Microsoft.eShopOnDapr.Web.Shopping.HttpAggregator.Services;

public interface ICatalogService
{
    Task<IEnumerable<CatalogItem>?> GetCatalogItemsAsync(IEnumerable<int> ids);

    Task<(int statusCode, string msg, BasketData data)> UpdateAllBasketAsync(UpdateBasketRequest data, string authorization);
}
