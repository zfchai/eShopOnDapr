namespace Microsoft.eShopOnDapr.Services.Ordering.API.IntegrationEvents;

public record ConfirmedOrderStockItem(string ProductId, bool HasStock);
