namespace Microsoft.eShopOnDapr.Services.Ordering.API.Actors;

public class OrderItemState
{
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; } = -1;
    public int Units { get; set; } = -1;
    public string PictureFileName { get; set; } = string.Empty;
}
