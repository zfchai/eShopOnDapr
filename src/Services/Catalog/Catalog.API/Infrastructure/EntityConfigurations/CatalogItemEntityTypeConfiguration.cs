using Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Catalog.API.Infrastructure.EntityConfigurations;

internal sealed class CatalogItemEntityTypeConfiguration(string tablePrefix) : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable($"{tablePrefix}_CatalogItems");

        builder.Property(item => item.Id)
            .UseHiLo("catalog_hilo")
            .IsRequired();

        builder.Property(item => item.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(item => item.Price)
            .HasPrecision(4, 2)
            .IsRequired();

        builder.Property(item => item.PictureFileName)
            .IsRequired();

        builder.HasOne(item => item.CatalogBrand)
            .WithMany()
            .HasForeignKey(item => item.CatalogBrandId);

        builder.HasOne(item => item.CatalogType)
            .WithMany()
            .HasForeignKey(item => item.CatalogTypeId);

        builder.HasData(
            new CatalogItem("0195e6e8-bb81-71e1-ba90-f41b62ef041e", ".NET Bot Black Hoodie", 19.5M, "1.png", "0195e6bf-aea8-7448-af5f-0fcdce95ad27", "0195e6c1-3413-736c-aeee-f48a48b5289a", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba90-fa28cb5b2609", ".NET Black & White Mug", 8.5M, "2.png", "0195e6bf-aea8-7448-af5f-0231107417bb", "0195e6c1-3413-736c-aeee-f48a48b5289a", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba90-ff7b4bd495ca", "Prism White T-Shirt", 12, "3.png", "0195e6bf-aea8-7448-af5f-0fcdce95ad27", "0195e6c1-3413-736c-aeee-fed5ac57e839", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-0352fb0e770a", ".NET Foundation T-shirt", 14.99M, "4.png", "0195e6bf-aea8-7448-af5f-0fcdce95ad27", "0195e6c1-3413-736c-aeee-f48a48b5289a", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-05bc9c9955c7", "Roslyn Red Pin", 8.5M, "5.png", "0195e6bf-aea8-7448-af5f-073c7bef56d6", "0195e6c1-3413-736c-aeee-fed5ac57e839", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-0bbf6aef962c", ".NET Blue Hoodie", 12, "6.png", "0195e6bf-aea8-7448-af5f-0fcdce95ad27", "0195e6c1-3413-736c-aeee-fed5ac57e839", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-0c4621949be1", "Roslyn Red T-Shirt", 12, "7.png", "0195e6bf-aea8-7448-af5f-0fcdce95ad27", "0195e6c1-3413-736c-aeee-fed5ac57e839", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-1307bfe426d8", "Kudu Purple Hoodie", 8.5M, "8.png", "0195e6bf-aea8-7448-af5f-0fcdce95ad27", "0195e6c1-3413-736c-aeee-fed5ac57e839", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-15d504eabf4a", "Cup<T> White Mug", 12, "9.png", "0195e6bf-aea8-7448-af5f-0231107417bb", "0195e6c1-3413-736c-aeee-fed5ac57e839", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-1b19b8a2343f", ".NET Foundation Pin", 9, "10.png", "0195e6bf-aea8-7448-af5f-08d72ff6a584", "0195e6c1-3413-736c-aeee-f48a48b5289a", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-1cbaa4eee3c3", "Cup<T> Pin", 8.5M, "11.png", "0195e6bf-aea8-7448-af5f-08d72ff6a584", "0195e6c1-3413-736c-aeee-fed5ac57e839", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-23bdadf243a9", "Prism White TShirt", 12, "12.png", "0195e6bf-aea8-7448-af5f-0fcdce95ad27", "0195e6c1-3413-736c-aeee-fed5ac57e839", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-279d1d673992", "Modern .NET Black & White Mug", 8.5M, "13.png", "0195e6bf-aea8-7448-af5f-0231107417bb", "0195e6c1-3413-736c-aeee-f48a48b5289a", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-2ba31e5a47fe", "Modern Cup<T> White Mug", 12, "14.png", "0195e6bf-aea8-7448-af5f-0231107417bb", "0195e6c1-3413-736c-aeee-fed5ac57e839", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-2e7cf37de906", "Dapr Cap", 9.99M, "15.png", "0195e6bf-aea8-7448-af5e-fc81fa53b243", "0195e6c1-3413-736c-aeee-f8032c3a77ba", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-33e91cc977fd", "Dapr Zipper Hoodie", 14.99M, "16.png", "0195e6bf-aea8-7448-af5f-0fcdce95ad27", "0195e6c1-3413-736c-aeee-f8032c3a77ba", 100),
            new CatalogItem("0195e6e8-bb81-71e1-ba91-3562253631af", "Dapr Logo Sticker", 1.99M, "17.png", "0195e6bf-aea8-7448-af5f-08d72ff6a584", "0195e6c1-3413-736c-aeee-f8032c3a77ba", 100));
    }
}
