namespace Byndyusoft.ArchitectureTesting.DependencyValidators.Tests.Fakes
{
    using System.Threading.Tasks;
    using Byndyusoft.Net.Kafka.Abstractions.Consuming;
    using Net.Kafka.Consuming;

    [KafkaMessageHandler(topic: "composer_assistant.entity.creation")]
    public class EntityCreationKafkaMessageHandler : KafkaMessageHandlerBase<EntityCreationMessage>
    {
        protected override Task Handle(EntityCreationMessage message) => Task.CompletedTask;
    }
}