namespace Microsoft.eShopOnDapr.Services.Basket.API.IntegrationEvents.EventHandling;

public class OrderStatusChangedToSubmittedIntegrationEventHandler(
    IBasketRepository repository)
        : IIntegrationEventHandler<OrderStatusChangedToSubmittedIntegrationEvent>
{
    public Task Handle(OrderStatusChangedToSubmittedIntegrationEvent @event) =>
        repository.DeleteBasketAsync(@event.BuyerId);
}



