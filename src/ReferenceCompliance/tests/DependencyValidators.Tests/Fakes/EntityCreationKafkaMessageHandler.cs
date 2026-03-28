namespace Byndyusoft.ArchitectureTesting.ReferenceCompliance.DependencyValidators.Tests.Fakes;

using System.Threading.Tasks;
using Byndyusoft.Net.Kafka.Abstractions.Consuming;
using Byndyusoft.Net.Kafka.Consuming;

[KafkaMessageHandler("composer_assistant.entity.creation")]
public class EntityCreationKafkaMessageHandler : KafkaMessageHandlerBase<EntityCreationMessage>
{
    protected override Task Handle(EntityCreationMessage message) => Task.CompletedTask;
}