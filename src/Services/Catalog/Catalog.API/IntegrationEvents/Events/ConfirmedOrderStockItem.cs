namespace Microsoft.eShopOnDapr.Services.Catalog.API.IntegrationEvents.Events;

/// <summary>
/// 确认订单库存项目（商品）
/// </summary>
/// <param name="ProductId"></param>
/// <param name="HasStock"></param>
public record ConfirmedOrderStockItem(string ProductId, bool HasStock);
