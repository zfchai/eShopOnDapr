namespace Microsoft.eShopOnDapr.Services.Catalog.API.IntegrationEvents.Events;

public record ConfirmedOrderStockItem(string ProductId, bool HasStock);
