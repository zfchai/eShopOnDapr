using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure;

public class OrderingDbContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public OrderingDbContext(DbContextOptions<OrderingDbContext> options)
        : base(options)
    {
        // No need for change tracking.
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dbSettings = this.GetService<ConnectionStrings>();
        var tablePrefix = dbSettings.TablePrefix.IfNullOrWhiteSpaceAs("eShorp");

        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
    }
}
