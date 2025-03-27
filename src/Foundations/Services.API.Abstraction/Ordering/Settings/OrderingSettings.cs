
namespace Microsoft.eShopOnDapr.Services.API.Abstraction.Ordering.Settings;

public class OrderingSettings
{
    public int GracePeriodTime { get; set; }

    public bool SendConfirmationEmail { get; set; }
}
