using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<CatalogBrand> CatalogBrands => Set<CatalogBrand>();
    public DbSet<CatalogItem> CatalogItems => Set<CatalogItem>();
    public DbSet<CatalogType> CatalogTypes => Set<CatalogType>();

    private const string _tablePrefix = "ConnectionStrings:TablePrefix";

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var dbSettings = this.GetService<IConfiguration>();
        var tablePrefix = dbSettings[_tablePrefix]!.IfNullOrWhiteSpaceAs("eShorp");

        builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration(tablePrefix));
        builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration(tablePrefix));
        builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration(tablePrefix));
    }     
}
