using Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetOrderByIdAsync(Guid orderId);
    Task<Order?> GetOrderByOrderNumberAsync(int orderNumber);
    Task<Order> AddOrGetOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    IAsyncEnumerable<OrderSummary?> GetOrdersFromBuyerAsync(string buyerId);
}
