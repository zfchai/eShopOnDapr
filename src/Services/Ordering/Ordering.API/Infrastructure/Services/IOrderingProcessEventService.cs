namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Services;

public interface IOrderingProcessEventService
{
    Task HandleAsync(UserCheckoutAcceptedIntegrationEvent integrationEvent);
    IOrderingProcessActor GetOrderingProcessActor(Guid orderId);

}
