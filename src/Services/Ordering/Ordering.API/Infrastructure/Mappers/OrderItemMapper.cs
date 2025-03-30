using Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Entities;
using Microsoft.eShopOnDapr.Services.Ordering.API.ViewModel.Response;

namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Mappers;

public static class OrderItemMapper
{
    public static OrderItemResp To(this OrderItem orderItem) => new
    (
        orderItem.ProductName,
        orderItem.Units,
        orderItem.UnitPrice,
        orderItem.PictureFileName
    );
}
