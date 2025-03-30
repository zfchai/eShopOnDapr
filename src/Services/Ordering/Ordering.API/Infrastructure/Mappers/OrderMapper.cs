using Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Entities;
using Microsoft.eShopOnDapr.Services.Ordering.API.ViewModel.Response;

namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Mappers;

public static class OrderMapper
{
    public static OrderResp To(this Order order) => new
    (
       order.OrderNumber,
       order.OrderDate,
       order.OrderStatus,
       order.Description,
       order.Address.Street,
       order.Address.City,
       order.Address.Country,
       order.OrderItems.Select(x => x.To()),
       order.GetTotal(),
       order.GetTotal()
    );
}
