using Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Services;

public class UpdateOrderStatusEventService(
    ILogger<UpdateOrderStatusEventService> logger,
    IServiceProvider sp
    ) : IUpdateOrderStatusEventService
{
    private readonly IOrderRepository _orderRepository = sp.GetRequiredService<IOrderRepository>();
    private readonly IHubContext<NotificationsHub> _hubContext = sp.GetRequiredService<IHubContext<NotificationsHub>>();
    private readonly IActorProxyFactory _actorProxyFactory = sp.GetRequiredService<IActorProxyFactory>();

    public async Task HandleAsync(
        OrderStatusChangedToSubmittedIntegrationEvent integrationEvent,
        OrderingSetting ordering,
        IEmailService emailService)
    {
        // Gets the order details from Actor state.
        var actorId = new ActorId(integrationEvent.OrderId.ToString());
        var orderingProcess = _actorProxyFactory.CreateActorProxy<IOrderingProcessActor>(
            actorId,
            nameof(OrderingProcessActor));
        //
        var actorOrder = await orderingProcess.GetOrderDetails();
        var readModelOrder = new Order(integrationEvent.OrderId, actorOrder);

        // Add the order to the read model so it can be queried from the API.
        // It may already exist if this event has been handled before (at-least-once semantics).
        readModelOrder = await _orderRepository.AddOrGetOrderAsync(readModelOrder);

        // Send a SignalR notification to the client.
        await SendNotificationAsync(readModelOrder.OrderNumber, integrationEvent.OrderStatus, integrationEvent.BuyerId);

        // Send a confirmation e-mail if enabled.
        if (ordering.SendConfirmationEmail)
        {
            await emailService.SendOrderConfirmationAsync(readModelOrder);
        }
    }

    public async Task UpdateReadModelAndSendNotificationAsync(
        Guid orderId, string orderStatus, string description, string buyerId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order is not null)
        {
            order.OrderStatus = orderStatus;
            order.Description = description;

            await _orderRepository.UpdateOrderAsync(order);
            await SendNotificationAsync(order.OrderNumber, orderStatus, buyerId);
        }
    }


    #region private method
    private Task SendNotificationAsync(
        int orderNumber, string orderStatus, string buyerId)
    {
        return _hubContext.Clients
            .Group(buyerId)
            .SendAsync("UpdatedOrderState", new { OrderNumber = orderNumber, Status = orderStatus });
    }
    #endregion

}
