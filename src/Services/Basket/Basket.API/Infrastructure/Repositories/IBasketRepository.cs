namespace Microsoft.eShopOnDapr.Services.Basket.API.Infrastructure.Repositories;

public interface IBasketRepository
{
    Task<CustomerBasket> GetBasketAsync(string customerId);
    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
    Task DeleteBasketAsync(string id);
}
