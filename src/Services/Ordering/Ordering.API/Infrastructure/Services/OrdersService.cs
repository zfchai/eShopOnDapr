namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Services;

public class OrdersService(
    IOrderRepository orderRepository,
    IIdentityService identityService
    ) : IOrdersService
{
    private readonly IOrderRepository _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    private readonly IIdentityService _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));

    public async Task<bool> CancelOrderAsync(int orderNumber) 
    {
        var orderingProcessActor = await GetOrderingProcessActorAsync(orderNumber);
        var result = await orderingProcessActor.CancelAsync();
        return result;
    }

    public async Task<bool> ShipOrderAsync(int orderNumber, string requestId) 
    {
        bool result = false;
        if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
        {
            var orderingProcessActor = await GetOrderingProcessActorAsync(orderNumber);
            result = await orderingProcessActor.ShipAsync();
        }

        return result;
    }

    public async Task<(bool state, Order? order)> GetOrderAsync(int orderNumber)
    {
        var buyerId = _identityService.GetUserIdentity();
        var order = await _orderRepository.GetOrderByOrderNumberAsync(orderNumber);
        if (order?.BuyerId == buyerId)
        {
            return (true, order);
        }

        return (false, order);
    }

    public async IAsyncEnumerable<OrderSummary?> GetOrdersAsync()
    {
        var buyerId = _identityService.GetUserIdentity();
        var orderSummaries = _orderRepository.GetOrdersFromBuyerAsync(buyerId);
        await foreach (var orderSummary in orderSummaries)
        {
            yield return orderSummary;
        }
    }

    private async Task<IOrderingProcessActor> GetOrderingProcessActorAsync(int orderNumber)
    {
        var order = await _orderRepository.GetOrderByOrderNumberAsync(orderNumber);
        if (order == null)
        {
            throw new ArgumentException($"Order with order number {orderNumber} not found.");
        }

        var actorId = new ActorId(order.Id.ToString());
        return ActorProxy.Create<IOrderingProcessActor>(actorId, nameof(OrderingProcessActor));
    }

}
