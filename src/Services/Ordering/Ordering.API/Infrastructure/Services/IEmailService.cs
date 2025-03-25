namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Services;

public interface IEmailService
{
    Task SendOrderConfirmationAsync(Order order);
}
