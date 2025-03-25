namespace Microsoft.eShopOnDapr.Services.Basket.API.Model;

public class CustomerBasket(string customerId)
{
    public string BuyerId { get; set; } = customerId;

    public List<BasketItem> Items { get; set; } = [];
}
