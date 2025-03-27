namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Services;

public class OrderingProcessEventService(
    ILogger<OrderingProcessEventService> logger,
    IActorProxyFactory actorProxyFactory
    ) : IOrderingProcessEventService
{
    public async Task HandleAsync(UserCheckoutAcceptedIntegrationEvent integrationEvent)
    {
        if (integrationEvent.RequestId != Guid.Empty)
        {
            var orderingProcess = GetOrderingProcessActor(integrationEvent.RequestId);

            await orderingProcess.SubmitAsync(
                integrationEvent.UserId, integrationEvent.UserEmail, integrationEvent.Street, integrationEvent.City,
                integrationEvent.State, integrationEvent.Country, integrationEvent.Basket);
        }
        else
        {
            logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", integrationEvent);
        }
    }

    public IOrderingProcessActor GetOrderingProcessActor(Guid orderId)
    {
        var actorId = new ActorId(orderId.ToString());
        return actorProxyFactory.CreateActorProxy<IOrderingProcessActor>(
            actorId,
            nameof(OrderingProcessActor));
    }
}
