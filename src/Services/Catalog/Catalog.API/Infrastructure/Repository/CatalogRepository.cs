using System.Security.Claims;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Repository;

public partial class CatalogRepository : ICatalogRepository
{
    private readonly ILogger<CatalogRepository> _logger;
    private readonly IServiceProvider _sp;
    private readonly CatalogDbContext _context;

    public CatalogRepository(
        ILogger<CatalogRepository> logger,
        IServiceProvider sp,
        CatalogDbContext context)
    {
        _logger = logger;
        _sp = sp;
        _context = context ?? throw new ArgumentNullException(nameof(context));
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    private IHttpContextAccessor HttpContextAccessor => _sp.GetRequiredService<IHttpContextAccessor>();
    private IEnumerable<Claim> Claims => HttpContextAccessor.HttpContext?.User.Claims!;
    private string OwnerId => Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;

}
