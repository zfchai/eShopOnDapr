namespace Microsoft.eShopOnDapr.BuildingBlocks.EventBus;

public class DaprEventBus(ILogger<DaprEventBus> logger, DaprClient dapr) : IEventBus
{
    private const string PubSubName = "eshopondapr-pubsub";

    public async Task PublishAsync(IntegrationEvent integrationEvent)
    {
        var topicName = integrationEvent.GetType().Name;

        logger.LogInformation(
            "Publishing event {@Event} to {PubsubName}.{TopicName}",
            integrationEvent,
            PubSubName,
            topicName);

        // We need to make sure that we pass the concrete type to PublishEventAsync,
        // which can be accomplished by casting the event to dynamic. This ensures
        // that all event fields are properly serialized.
        await dapr.PublishEventAsync(PubSubName, topicName, (object)integrationEvent);
    }
}
