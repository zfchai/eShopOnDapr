namespace Microsoft.eShopOnDapr.Services.Ordering.API.IntegrationEvents;

public record GracePeriodConfirmedIntegrationEvent(Guid OrderId) : IntegrationEvent;
