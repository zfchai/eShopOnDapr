namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Basket.ViewModel.Request;

public record BasketCheckoutReq(
    string UserEmail,
    string City,
    string Street,
    string State,
    string Country,
    string CardNumber,
    string CardHolderName,
    DateTime CardExpiration,
    string CardSecurityCode
);