namespace Microsoft.eShopOnDapr.BuildingBlocks.EventBus.Events;

public record IntegrationEvent
{
    public Guid Id { get; }

    public DateTimeOffset CreationDate { get; }

    public IntegrationEvent()
    {
        Id = Guid.CreateVersion7();
        CreationDate = DateTimeOffset.UtcNow;
    }
}
