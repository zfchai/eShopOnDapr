
namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Request;

public class OrderConfirmationReq
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public string BuyerEmail { get; set; } = null!;
}
