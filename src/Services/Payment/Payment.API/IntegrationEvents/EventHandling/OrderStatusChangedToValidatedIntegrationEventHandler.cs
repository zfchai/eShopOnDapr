namespace Microsoft.eShopOnDapr.Services.Payment.API.IntegrationEvents.EventHandling;

public class OrderStatusChangedToValidatedIntegrationEventHandler(
    ILogger<OrderStatusChangedToValidatedIntegrationEventHandler> logger,
    IOptions<PaymentSettings> settings,
    IEventBus eventBus
    ) : IIntegrationEventHandler<OrderStatusChangedToValidatedIntegrationEvent>
{
    private readonly PaymentSettings _settings = settings.Value;

    public async Task Handle(OrderStatusChangedToValidatedIntegrationEvent @event)
    {
        IntegrationEvent orderPaymentIntegrationEvent;

        // Business feature comment:
        // When OrderStatusChangedToValidated Integration Event is handled.
        // Here we're simulating that we'd be performing the payment against any payment gateway.
        // Instead of a real payment we just take the MaxOrderTotal to simulate payment approval.
        // The payment can be successful or it can fail

        await Task.Delay(3000); // Checking with the bank 😉

        if (_settings.PaymentSucceeded &&
            (!_settings.MaxOrderTotal.HasValue || @event.Total < _settings.MaxOrderTotal ))
        {
            orderPaymentIntegrationEvent = new OrderPaymentSucceededIntegrationEvent(@event.OrderId);
        }
        else
        {
            logger.LogWarning(
                "Payment for ${Total} rejected for order {OrderId} because of service configuration",
                @event.Total,
                @event.OrderId);

            orderPaymentIntegrationEvent = new OrderPaymentFailedIntegrationEvent(@event.OrderId);
        }

        await eventBus.PublishAsync(orderPaymentIntegrationEvent);
    }
}
