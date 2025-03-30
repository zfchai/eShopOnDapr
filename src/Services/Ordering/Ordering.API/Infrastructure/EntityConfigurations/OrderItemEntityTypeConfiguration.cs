using Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.EntityConfigurations;

public class OrderItemEntityTypeConfiguration(string tablePrefix) : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable($"{tablePrefix}_OrderItems");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .UseHiLo("orderitemseq");

        builder.Property(item => item.UnitPrice)
            .HasPrecision(4, 2);
    }
}
