namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.ViewModel.Response;

//TODO
public record OrderSummaryResp(
    int OrderNumber,
    DateTime Date,
    string Status,
    decimal Total);
