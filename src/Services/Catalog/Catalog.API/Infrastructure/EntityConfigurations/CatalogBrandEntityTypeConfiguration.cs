using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.EntityConfigurations;

public sealed class CatalogBrandEntityTypeConfiguration(string tablePrefix) : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder.ToTable($"{tablePrefix}_CatalogBrands");

        builder.HasKey(brand => brand.Id);

        builder.Property(brand => brand.Id)
            .UseHiLo("catalog_brand_hilo")
            .IsRequired();

        builder.Property(brand => brand.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            new CatalogBrand("0195e6c1-3413-736c-aeee-f48a48b5289a", ".NET"),
            new CatalogBrand("0195e6c1-3413-736c-aeee-f8032c3a77ba", "Dapr"),
            new CatalogBrand("0195e6c1-3413-736c-aeee-fed5ac57e839", "Other"));
    }
}
