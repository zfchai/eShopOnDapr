namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Models;

public record OrderSummary(
    Guid Id,
    int OrderNumber,
    DateTime OrderDate,
    string OrderStatus,
    decimal Total);
