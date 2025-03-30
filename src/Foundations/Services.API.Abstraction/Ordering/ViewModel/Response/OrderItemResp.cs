namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Response;


public class OrderItemResp 
{
    public int Id { get; set; }
    public Guid OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int Units { get; set; }
    public string PictureFileName { get; set; } = null!;
}
