namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.EntityConfigurations;
    
public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.HasAlternateKey(o => o.OrderNumber);

        builder.Property(o => o.OrderNumber)
            .UseHiLo("orderseq");

        builder
            .OwnsOne(o => o.Address, a =>
            {
                a.WithOwner();
            });
    }
}
