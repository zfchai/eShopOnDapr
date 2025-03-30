using Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Request;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.Services;

public interface IEmailService
{
    Task SendOrderConfirmationAsync(OrderConfirmationReq order);
}
