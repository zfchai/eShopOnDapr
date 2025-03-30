using Microsoft.eShopOnDapr.Services.API.Abstraction.Basket.ViewModel.Model;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Basket.ViewModel.Request;

public class CustomerBasketReq(string customerId)
{
    public string BuyerId { get; set; } = customerId;

    public List<BasketItem> Items { get; set; } = [];
}
