using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure.Entities;

namespace Microsoft.eShopOnDapr.Services.Ordering.API.Infrastructure;

public class OrderingDbContext : DbContext
{
    public OrderingDbContext(DbContextOptions<OrderingDbContext> options)
        : base(options)
    {
        // No need for change tracking.
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    private const string _tablePrefix = "ConnectionStrings:TablePrefix";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dbSettings = this.GetService<IConfiguration>();
        var tablePrefix = dbSettings[_tablePrefix]!.IfNullOrWhiteSpaceAs("eShorp");

        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration(tablePrefix));
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration(tablePrefix));
    }
}
