namespace Microsoft.eShopOnDapr.Services.Ordering.API.Actors;

public class OrderStatus(int id, string name)
{
    public static readonly OrderStatus New = new(0, nameof(New));
    public static readonly OrderStatus Submitted = new(1, nameof(Submitted));
    public static readonly OrderStatus AwaitingStockValidation = new(2, nameof(AwaitingStockValidation));
    public static readonly OrderStatus Validated = new(3, nameof(Validated));
    public static readonly OrderStatus Paid = new(4, nameof(Paid));
    public static readonly OrderStatus Shipped = new(5, nameof(Shipped));
    public static readonly OrderStatus Cancelled = new(6, nameof(Cancelled));

    public int Id { get; set; } = id;

    public string Name { get; set; } = name;

    public OrderStatus()
        : this(New.Id, New.Name)
    {
    }
}
