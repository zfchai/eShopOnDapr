namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Services;

public interface IOrdersService
{
    Task<bool> CancelOrderAsync(int orderNumber);
    Task<bool> ShipOrderAsync(int orderNumber, string requestId);
    Task<(bool state, Order? order)> GetOrderAsync(int orderNumber);
    IAsyncEnumerable<OrderSummary?> GetOrdersAsync();
}
