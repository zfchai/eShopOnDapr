namespace Microsoft.eShopOnDapr.Services.Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public sealed class OrderingProcessEventController(
    ILogger<OrderingProcessEventController> logger,
    IOrderingProcessEventService orderingProcessEventService) : ControllerBase
{
    private const string DAPR_PUBSUB_NAME = "eshopondapr-pubsub";

    [HttpPost("UserCheckoutAccepted")]
    [Topic(DAPR_PUBSUB_NAME, "UserCheckoutAcceptedIntegrationEvent")]
    public async Task HandleAsync(UserCheckoutAcceptedIntegrationEvent integrationEvent)
    {
        await orderingProcessEventService.HandleAsync(integrationEvent);
    }

    [HttpPost("OrderStockConfirmed")]
    [Topic(DAPR_PUBSUB_NAME, "OrderStockConfirmedIntegrationEvent")]
    public async Task HandleAsync(OrderStockConfirmedIntegrationEvent integrationEvent)
    {
        await orderingProcessEventService.GetOrderingProcessActor(integrationEvent.OrderId)
            .NotifyStockConfirmedAsync();
    }

    [HttpPost("OrderStockRejected")]
    [Topic(DAPR_PUBSUB_NAME, "OrderStockRejectedIntegrationEvent")]
    public async Task HandleAsync(OrderStockRejectedIntegrationEvent integrationEvent)
    {
        var outOfStockItems = integrationEvent.OrderStockItems
            .FindAll(c => !c.HasStock)
            .Select(c => c.ProductId)
            .ToList();

        await orderingProcessEventService.GetOrderingProcessActor(integrationEvent.OrderId)
            .NotifyStockRejectedAsync(outOfStockItems);
    }

    [HttpPost("OrderPaymentSucceeded")]
    [Topic(DAPR_PUBSUB_NAME, "OrderPaymentSucceededIntegrationEvent")]
    public async Task HandleAsync(OrderPaymentSucceededIntegrationEvent integrationEvent)
    {
        await orderingProcessEventService.GetOrderingProcessActor(integrationEvent.OrderId)
            .NotifyPaymentSucceededAsync();
    }

    [HttpPost("OrderPaymentFailed")]
    [Topic(DAPR_PUBSUB_NAME, "OrderPaymentFailedIntegrationEvent")]
    public async Task HandleAsync(OrderPaymentFailedIntegrationEvent integrationEvent)
    {
        await orderingProcessEventService.GetOrderingProcessActor(integrationEvent.OrderId)
            .NotifyPaymentFailedAsync();
    }

}
