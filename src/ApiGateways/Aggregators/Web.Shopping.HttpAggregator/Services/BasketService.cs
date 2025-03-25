namespace Microsoft.eShopOnDapr.Web.Shopping.HttpAggregator.Services;

public class BasketService(HttpClient httpClient) : IBasketService
{
    public async Task UpdateAsync(BasketData currentBasket, string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/basket")
        {
            Content = JsonContent.Create(currentBasket)
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}

