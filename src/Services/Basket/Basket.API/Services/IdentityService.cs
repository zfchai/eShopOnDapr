namespace Microsoft.eShopOnDapr.Services.Basket.API.Services;

public class IdentityService(IHttpContextAccessor context) : IIdentityService
{
    public string GetUserIdentity()
    {
        return context.HttpContext?.User.FindFirst("sub")?.Value ?? "";
    }
}
