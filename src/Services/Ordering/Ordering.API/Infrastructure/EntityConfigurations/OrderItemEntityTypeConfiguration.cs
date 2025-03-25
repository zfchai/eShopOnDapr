namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.EntityConfigurations;

public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .UseHiLo("orderitemseq");

        builder.Property(item => item.UnitPrice)
            .HasPrecision(4, 2);
    }
}
