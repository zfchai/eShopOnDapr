using Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Mappers;

public static class OrderMapper
{
    public static OrderResp To(this Order order) => new()
    {
        Id = order.Id,
        OrderNumber = order.OrderNumber,
        OrderDate = order.OrderDate,
        OrderStatus = order.OrderStatus,
        Description = order.Description,
        Address = order.Address,
        BuyerId = order.BuyerId,
        BuyerEmail = order.BuyerEmail,
        OrderItems = order.OrderItems.Select(x => x.To())
    };
}
