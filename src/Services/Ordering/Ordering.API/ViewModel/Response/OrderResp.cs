namespace Microsoft.eShopOnDapr.Services.Ordering.API.ViewModel.Response;

// TODO
public record OrderResp(
    int OrderNumber,
    DateTime Date,
    string Status,
    string Description,
    string Street,
    string City,
    string Country,
    IEnumerable<OrderItemResp> OrderItems,
    decimal Subtotal, // TODO Remove subtotal
    decimal Total);