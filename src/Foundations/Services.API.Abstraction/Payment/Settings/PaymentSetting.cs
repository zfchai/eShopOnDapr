namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Payment.Settings;

public class PaymentSetting
{
    public bool PaymentSucceeded { get; set; } = true;

    public decimal? MaxOrderTotal { get; set; } = null!;
}
