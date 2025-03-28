using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.eShopOnDapr.Services.Identity.API.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{

    private const string _tablePrefix = "ConnectionStrings:TablePrefix";

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var dbSettings = this.GetService<IConfiguration>();
        var tablePrefix = dbSettings[_tablePrefix]!.IfNullOrWhiteSpaceAs("eShorp");

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
