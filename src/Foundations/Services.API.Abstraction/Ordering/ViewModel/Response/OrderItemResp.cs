namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Response;


public class OrderItemResp 
{
    public string Id { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public string ProductId { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Units { get; set; }
    public string PictureFileName { get; set; } = string.Empty;
}
