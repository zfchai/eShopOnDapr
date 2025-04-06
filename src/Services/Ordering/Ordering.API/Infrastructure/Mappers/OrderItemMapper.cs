using Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Entities;
namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Mappers;

public static class OrderItemMapper
{
    public static OrderItemResp To(this OrderItem orderItem) => new()
    {
        Id = orderItem.Id,
        OrderId = orderItem.OrderId,
        ProductId = orderItem.ProductId,
        ProductName = orderItem.ProductName,
        UnitPrice =  orderItem.UnitPrice,
        Units = orderItem.Units,
        PictureFileName = orderItem.PictureFileName
    };
}
