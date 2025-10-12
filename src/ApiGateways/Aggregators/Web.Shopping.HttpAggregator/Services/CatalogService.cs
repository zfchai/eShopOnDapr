namespace Microsoft.eShopOnDapr.Web.Shopping.HttpAggregator.Services;

public class CatalogService(IServiceProvider sp, HttpClient httpClient) : ICatalogService
{
    private readonly IBasketService basketService = sp.GetRequiredService<IBasketService>();

    public Task<IEnumerable<CatalogItem>?> GetCatalogItemsAsync(IEnumerable<string> ids)
    {
        var requestUri = $"api/v1/catalog/items/by_ids?ids={string.Join(",", ids)}";

        return httpClient.GetFromJsonAsync<IEnumerable<CatalogItem>>(requestUri);
    }

    public async Task<(int statusCode, string msg, BasketData data)> UpdateAllBasketAsync(UpdateBasketRequest data, string authorization) 
    {
        BasketData basket = new();

        if(data.Items is not null)
        {
            // Get the item details from the catalog API.
            var productIds = data.Items.Select(x => x.ProductId);
            var catalogItems = await GetCatalogItemsAsync(productIds);
            if (catalogItems == null)
            {
                string msg = "Catalog items were not available for the specified items in the basket.";
                return (StatusCodes.Status400BadRequest, msg, basket);
            }

            // Check item availability and prices; store results in basket object.
            basket = CreateValidatedBasket(data.Items, catalogItems);
        }

        // Save the updated shopping basket.
        await basketService.UpdateAsync(basket, authorization.Substring("Bearer ".Length));
        return (StatusCodes.Status200OK, "ok", basket);
    }

    private BasketData CreateValidatedBasket(
        IEnumerable<UpdateBasketRequestItemData> basketItems,
        IEnumerable<CatalogItem> catalogItems)
    {
        var basket = new BasketData();

        var itemsCalculated = basketItems.GroupBy(
            x => x.ProductId,
            x => x,
            (k, i) => new UpdateBasketRequestItemData(k, i.Sum(j => j.Quantity)));

        foreach (var bitem in itemsCalculated)
        {
            var catalogItem = catalogItems.SingleOrDefault(ci => ci.Id == bitem.ProductId);
            if (catalogItem is not null)
            {
                basket.Items.Add(new BasketDataItem(
                    catalogItem.Id,
                    catalogItem.Name,
                    catalogItem.Price,
                    bitem.Quantity,
                    catalogItem.PictureFileName));
            }
        }

        return basket;
    }

}
