namespace Microsoft.eShopOnDapr.Services.Ordering.API.ViewModel.Response;

//TODO
public record OrderSummaryResp(
    int OrderNumber,
    DateTime Date,
    string Status,
    decimal Total);
