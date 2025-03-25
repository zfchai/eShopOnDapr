namespace Microsoft.eShopOnDapr.Web.Shopping.HttpAggregator.Services;

public class CatalogService(HttpClient httpClient) : ICatalogService
{
    public Task<IEnumerable<CatalogItem>?> GetCatalogItemsAsync(IEnumerable<int> ids)
    {
        var requestUri = $"api/v1/catalog/items/by_ids?ids={string.Join(",", ids)}";

        return httpClient.GetFromJsonAsync<IEnumerable<CatalogItem>>(requestUri);
    }
}
