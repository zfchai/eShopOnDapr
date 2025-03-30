using Microsoft.eShopOnDapr.Services.API.Abstraction.Basket.ViewModel.Request;
using Microsoft.eShopOnDapr.Services.API.Abstraction.Basket.ViewModel.Response;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Basket.Services;

public interface IIdentityService
{
    string GetUserIdentity();

    Task<CustomerBasketResp> GetBasketAsync();

    Task<CustomerBasketResp> UpdateBasketAsync(CustomerBasketReq value);

    Task<int> CheckoutAsync(BasketCheckoutReq basketCheckout, string requestId);

    Task DeleteBasketAsync();
}
