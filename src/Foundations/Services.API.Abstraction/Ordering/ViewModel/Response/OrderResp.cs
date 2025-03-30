using Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Models;

namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Response;

public class OrderResp 
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Address Address { get; set; } = null!;
    public string BuyerId { get; set; } = null!;
    public string BuyerEmail { get; set; } = null!;
    public IEnumerable<OrderItemResp> OrderItems { get; set; } = [];

    public decimal GetTotal() => OrderItems.Sum(o => o.Units * o.UnitPrice);
}
