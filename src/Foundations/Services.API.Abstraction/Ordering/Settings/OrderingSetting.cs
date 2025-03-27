
namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.Settings;

public class OrderingSetting
{
    public int GracePeriodTime { get; set; }

    public bool SendConfirmationEmail { get; set; }
}
