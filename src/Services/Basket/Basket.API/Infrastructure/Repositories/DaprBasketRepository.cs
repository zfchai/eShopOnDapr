namespace Microsoft.eShopOnDapr.Services.Basket.API.Infrastructure.Repositories;

public class DaprBasketRepository(ILogger<DaprBasketRepository> logger, DaprClient daprClient) : IBasketRepository
{
    private const string StateStoreName = "eshopondapr-statestore";

    public Task DeleteBasketAsync(string id) =>
        daprClient.DeleteStateAsync(StateStoreName, id);

    public Task<CustomerBasketResp> GetBasketAsync(string customerId) =>
        daprClient.GetStateAsync<CustomerBasketResp>(StateStoreName, customerId);

    public async Task<CustomerBasketResp> UpdateBasketAsync(CustomerBasketReq basket)
    {
        var state = await daprClient.GetStateEntryAsync<CustomerBasketResp>(StateStoreName, basket.BuyerId);
        state.Value = new CustomerBasketResp(basket.BuyerId) 
        { 
            Items = basket.Items 
        };

        await state.SaveAsync();

        logger.LogInformation("Basket item persisted successfully.");

        return await GetBasketAsync(basket.BuyerId);
    }
}
