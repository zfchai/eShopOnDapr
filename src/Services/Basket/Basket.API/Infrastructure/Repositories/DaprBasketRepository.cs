namespace Microsoft.eShopOnDapr.Services.Basket.API.Infrastructure.Repositories;

public class DaprBasketRepository(ILogger<DaprBasketRepository> logger, DaprClient daprClient) : IBasketRepository
{
    private const string StateStoreName = "eshopondapr-statestore";

    public Task DeleteBasketAsync(string id) =>
        daprClient.DeleteStateAsync(StateStoreName, id);

    public Task<CustomerBasket> GetBasketAsync(string customerId) =>
        daprClient.GetStateAsync<CustomerBasket>(StateStoreName, customerId);

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var state = await daprClient.GetStateEntryAsync<CustomerBasket>(StateStoreName, basket.BuyerId);
        state.Value = basket;

        await state.SaveAsync();

        logger.LogInformation("Basket item persisted successfully.");

        return await GetBasketAsync(basket.BuyerId);
    }
}
