namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Models;

public record OrderSummary(
    Guid Id,
    int OrderNumber,
    DateTime OrderDate,
    string OrderStatus,
    decimal Total);
