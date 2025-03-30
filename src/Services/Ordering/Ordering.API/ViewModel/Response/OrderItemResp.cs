namespace Microsoft.eShopOnDapr.Services.Ordering.API.ViewModel.Response;

// TODO
public record class OrderItemResp(
    string ProductName, 
    int Units, decimal 
    UnitPrice, 
    string PictureUrl);
