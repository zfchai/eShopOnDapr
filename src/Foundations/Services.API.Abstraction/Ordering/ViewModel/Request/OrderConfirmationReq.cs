
namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Request;

public class OrderConfirmationReq
{
    public Guid Id { get; private set; }
    public int OrderNumber { get; private set; }
    public string BuyerEmail { get; set; } = null!;
}
