namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Services;

public interface IUpdateOrderStatusEventService
{
    Task HandleAsync(
        OrderStatusChangedToSubmittedIntegrationEvent integrationEvent,
        OrderingSettings ordering,
        IEmailService emailService);

    Task UpdateReadModelAndSendNotificationAsync(
        Guid orderId, string orderStatus, string description, string buyerId);

}
