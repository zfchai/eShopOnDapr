namespace Microsoft.eShopOnDapr.BlazorClient.Basket;

public class BasketClient(HttpClient httpClient)
{
    public async Task<IEnumerable<BasketItem>> GetItemsAsync()
    {
        var basket = await httpClient.GetFromJsonAsync<BasketData>(
            "b/api/v1/basket/");

        return basket!.Items;
    }

    public async Task<IEnumerable<BasketItem>> SaveItemsAsync(IEnumerable<BasketItem> items)
    {
        var request = new BasketData(items);

        // Save items is a request to the Aggregator service.
        var response = await httpClient.PostAsJsonAsync(
            "api/v1/basket/",
            request);

        response.EnsureSuccessStatusCode();

        var basketData = await response.Content.ReadFromJsonAsync<BasketData>();
        return basketData!.Items;
    }

    public async Task CheckoutAsync(BasketCheckout basketCheckout)
    {
        var response = await httpClient.PostAsJsonAsync(
            "b/api/v1/basket/checkout",
            basketCheckout);

        response.EnsureSuccessStatusCode();
    }
}
