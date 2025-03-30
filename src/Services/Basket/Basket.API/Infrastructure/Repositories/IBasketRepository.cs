namespace Microsoft.eShopOnDapr.Services.Basket.API.Infrastructure.Repositories;

public interface IBasketRepository
{
    Task<CustomerBasketResp> GetBasketAsync(string customerId);
    Task<CustomerBasketResp> UpdateBasketAsync(CustomerBasketReq basket);
    Task DeleteBasketAsync(string id);
}
