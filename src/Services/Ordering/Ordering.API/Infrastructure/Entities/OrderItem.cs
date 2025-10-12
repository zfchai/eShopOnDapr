namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Entities;

public class OrderItem
{
    public string Id { get; set; }
    public Guid OrderId { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Units { get; set; }
    public string PictureFileName { get; set; }

    public OrderItem()
    {
        OrderId = Guid.Empty;
        Id = string.Empty;
        ProductId = string.Empty;
        ProductName = string.Empty;
        PictureFileName = string.Empty;
    }

    public OrderItem(OrderItemState state)
    {
        Id = Guid.CreateVersion7().ToString();
        ProductId = state.ProductId;
        ProductName = state.ProductName;
        UnitPrice = state.UnitPrice;
        Units = state.Units;
        PictureFileName = state.PictureFileName;
    }
}
