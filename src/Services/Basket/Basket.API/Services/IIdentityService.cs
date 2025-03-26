namespace Microsoft.eShopOnDapr.Services.Basket.API.Services;

public interface IIdentityService
{
    string GetUserIdentity();

    Task<CustomerBasket> GetBasketAsync();

    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket value);

    Task<int> CheckoutAsync(BasketCheckout basketCheckout, string requestId);

    Task DeleteBasketAsync();
}
