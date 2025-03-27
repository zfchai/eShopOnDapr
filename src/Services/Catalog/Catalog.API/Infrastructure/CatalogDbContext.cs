using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<CatalogBrand> CatalogBrands => Set<CatalogBrand>();
    public DbSet<CatalogItem> CatalogItems => Set<CatalogItem>();
    public DbSet<CatalogType> CatalogTypes => Set<CatalogType>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var dbSettings = this.GetService<ConnectionStrings>();
        var tablePrefix = dbSettings.TablePrefix.IfNullOrWhiteSpaceAs("eShorp");

        builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration(tablePrefix));
        builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration(tablePrefix));
        builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration(tablePrefix));
    }     
}
