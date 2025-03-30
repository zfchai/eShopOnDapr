using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.EntityConfigurations;

internal sealed class CatalogTypeEntityTypeConfiguration(string tablePrefix) : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable($"{tablePrefix}_CatalogTypes");

        builder.HasKey(type => type.Id);

        builder.Property(type => type.Id)
            .UseHiLo("catalog_type_hilo")
            .IsRequired();

        builder.Property(type => type.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            new CatalogType("0195e6bf-aea8-7448-af5e-fc81fa53b243", "Cap"),
            new CatalogType("0195e6bf-aea8-7448-af5f-0231107417bb", "Mug"),
            new CatalogType("0195e6bf-aea8-7448-af5f-073c7bef56d6", "Pin"),
            new CatalogType("0195e6bf-aea8-7448-af5f-08d72ff6a584", "Sticker"),
            new CatalogType("0195e6bf-aea8-7448-af5f-0fcdce95ad27", "T-Shirt"));
    }
}
