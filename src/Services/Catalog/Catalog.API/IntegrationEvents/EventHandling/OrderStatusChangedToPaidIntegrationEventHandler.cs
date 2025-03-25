namespace Microsoft.eShopOnDapr.Services.Catalog.API.IntegrationEvents.EventHandling;

public class OrderStatusChangedToPaidIntegrationEventHandler(CatalogDbContext context) : 
    IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
{
    public async Task Handle(OrderStatusChangedToPaidIntegrationEvent @event)
    {
        //we're not blocking stock/inventory
        foreach (var orderStockItem in @event.OrderStockItems)
        {
            var catalogItem = context.CatalogItems.Find(orderStockItem.ProductId);
            if (catalogItem != null)
            {
                catalogItem.RemoveStock(orderStockItem.Units);
            }
        }

        await context.SaveChangesAsync();
    }
}
