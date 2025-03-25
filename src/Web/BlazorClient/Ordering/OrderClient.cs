namespace Microsoft.eShopOnDapr.BlazorClient.Ordering;

public class OrderClient(HttpClient httpClient)
{
    public Task<IEnumerable<OrderSummary>> GetOrdersAsync()
        => httpClient.GetFromJsonAsync<IEnumerable<OrderSummary>>(
            "o/api/v1/orders/")!;

    public Task<Order> GetOrderDetailsAsync(int orderNumber)
        => httpClient.GetFromJsonAsync<Order>(
            $"o/api/v1/orders/{orderNumber}")!;

    public async Task CancelOrderAsync(int orderNumber)
    {
        var response = await httpClient.PutAsync(
            $"o/api/v1/orders/{orderNumber}/cancel",
            null);

        response.EnsureSuccessStatusCode();
    }
}
