
namespace Microsoft.eShopOnDapr.Services.Ordering.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UpdateOrderStatusEventController(
    ILogger<UpdateOrderStatusEventController> logger,
    IUpdateOrderStatusEventService updateOrderStatusEventService
    ) : ControllerBase
{
    private const string DAPR_PUBSUB_NAME = "eshopondapr-pubsub";

   
    [HttpPost("OrderStatusChangedToSubmitted")]
    [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToSubmittedIntegrationEvent))]
    public async Task HandleAsync(
        OrderStatusChangedToSubmittedIntegrationEvent integrationEvent,
        [FromServices] IOptions<OrderingSetting> setting,
        [FromServices] IEmailService emailService)
    {
        await updateOrderStatusEventService.HandleAsync(integrationEvent, setting.Value, emailService);
    }

    [HttpPost("OrderStatusChangedToAwaitingStockValidation")]
    [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToAwaitingStockValidationIntegrationEvent))]
    public async Task HandleAsync(
        OrderStatusChangedToAwaitingStockValidationIntegrationEvent integrationEvent)
    {
        // Save the updated status in the read model and notify the client via SignalR.
        await updateOrderStatusEventService.UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
            integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerId);
    }

    [HttpPost("OrderStatusChangedToValidated")]
    [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToValidatedIntegrationEvent))]
    public async Task HandleAsync(
        OrderStatusChangedToValidatedIntegrationEvent integrationEvent)
    {
        // Save the updated status in the read model and notify the client via SignalR.
        await updateOrderStatusEventService.UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
            integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerId);
    }

    [HttpPost("OrderStatusChangedToPaid")]
    [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToPaidIntegrationEvent))]
    public async Task HandleAsync(
        OrderStatusChangedToPaidIntegrationEvent integrationEvent)
    {
        // Save the updated status in the read model and notify the client via SignalR.
        await updateOrderStatusEventService.UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
            integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerId);
    }

    [HttpPost("OrderStatusChangedToShipped")]
    [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToShippedIntegrationEvent))]
    public async Task HandleAsync(
        OrderStatusChangedToShippedIntegrationEvent integrationEvent)
    {
        // Save the updated status in the read model and notify the client via SignalR.
        await updateOrderStatusEventService.UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
            integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerId);
    }

    [HttpPost("OrderStatusChangedToCancelled")]
    [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToCancelledIntegrationEvent))]
    public async Task HandleAsync(
        OrderStatusChangedToCancelledIntegrationEvent integrationEvent)
    {
        // Save the updated status in the read model and notify the client via SignalR.
        await updateOrderStatusEventService.UpdateReadModelAndSendNotificationAsync(integrationEvent.OrderId,
            integrationEvent.OrderStatus, integrationEvent.Description, integrationEvent.BuyerId);
    }

}
