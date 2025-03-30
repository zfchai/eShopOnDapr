using Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Models;
using Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Response;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.Services;

public interface IOrdersService
{
    Task<bool> CancelOrderAsync(int orderNumber);
    Task<bool> ShipOrderAsync(int orderNumber, string requestId);
    Task<(bool state, OrderResp? order)> GetOrderAsync(int orderNumber);
    IAsyncEnumerable<OrderSummary?> GetOrdersAsync();
}
