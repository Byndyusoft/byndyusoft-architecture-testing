namespace Byndyusoft.ArchitectureTesting.DependencyValidators.Tests.Fakes;

using System.Threading.Tasks;
using Net.Kafka.Abstractions.Consuming;
using Net.Kafka.Consuming;

[KafkaMessageHandler("composer_assistant.entity.creation")]
public class EntityCreationKafkaMessageHandler : KafkaMessageHandlerBase<EntityCreationMessage>
{
    protected override Task Handle(EntityCreationMessage message) => Task.CompletedTask;
}