namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Services;

public class IdentityService(IHttpContextAccessor context) : IIdentityService
{
    private IHttpContextAccessor _context = context ?? throw new ArgumentNullException(nameof(context));

    public string GetUserIdentity() =>
        _context.HttpContext?.User?.FindFirst("sub")?.Value ?? string.Empty;
}
